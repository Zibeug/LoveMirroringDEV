/*
 *      Auteur : Tim Allemann
 *      2020.05.08
 *      Rajoute les claims identity server 4 au claims d'identity
 *      Permet d'utiliser des policy pour gérer les accès des controlleurs
 */

using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Models;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace Api.Services.RolesAndClaims
{
    public class KarekeClaimsTransformer : IClaimsTransformation
    {
        private readonly IConfiguration _configuration;
        private HttpClient httpClient;

        public KarekeClaimsTransformer(IConfiguration configuration)
        {
            this._configuration = configuration;
            httpClient = new HttpClient();
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                await GetTokenFromIdentityServer4();
                Claim userId = principal.FindFirst("sub"); 
                if (principal.FindFirst("Role") == null && userId != null)
                {
                    using (var context = new LoveMirroringContext())
                    {
                        var roles = context.AspNetUserRoles.Where(a => a.UserId == userId.Value).Select(r => r.RoleId); foreach (var role in roles)
                        {
                            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(JwtClaimTypes.Role, role, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"));
                        }
                    }
                }
            }
            return await Task.FromResult(principal);
        }

        public async Task GetTokenFromIdentityServer4()
        {
            // discover endpoints from metadata point sur apsettings
            var disco = await httpClient.GetDiscoveryDocumentAsync(this._configuration["URLIdentityServer4"]);
            if (disco.IsError)
            {
                return;
            }

            // request token
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "mvc",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                return;
            }
            httpClient.SetBearerToken(tokenResponse.AccessToken);
        }
    }

}
