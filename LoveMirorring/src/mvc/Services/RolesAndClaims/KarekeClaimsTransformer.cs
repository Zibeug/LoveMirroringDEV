/*
 *      Auteur : Tim Allemann
 *      2020.05.08
 *      Rajoute les claims identity server 4 au claims d'identity
 *      Permet d'utiliser des policy pour gérer les accès des controlleurs
 */

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using mvc.Models;
using Newtonsoft.Json;

namespace mvc.Services.RolesAndClaims
{
    public class KarekeClaimsTransformer : Microsoft.AspNetCore.Authentication.IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                Claim userId = principal.FindFirst("sub");

                if (principal.FindFirst("role") == null && userId != null)
                {
                    // Préparation de l'appel à l'API
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                    string content = await client.GetStringAsync(Startup.Configuration["URLAPI"] + "api/Account/GetRole/" + userId.Value);

                    List<string> roles = JsonConvert.DeserializeObject<List<string>>(content);
                    foreach (var role in roles)
                    {
                        ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(JwtClaimTypes.Role, role,
                            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"));
                    }
                }
            }
            return await Task.FromResult(principal);
        }
    }
}
