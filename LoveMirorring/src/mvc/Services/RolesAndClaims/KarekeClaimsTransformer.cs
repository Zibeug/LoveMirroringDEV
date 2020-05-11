/*
 *      Auteur : Tim Allemann
 *      2020.05.08
 *      Rajoute les claims identity server 4 au claims d'identity
 *      Permet d'utiliser des policy pour gérer les accès des controlleurs
 */

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using mvc.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace mvc.Services.RolesAndClaims
{
    using IdentityModel.Client;
    using Microsoft.Extensions.Configuration;
    using System;

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

                Claim userId = principal.FindFirst("sub"); if (principal.FindFirst("Role") == null && userId != null)
                {
                    string content = await httpClient.GetStringAsync(_configuration["URLAPI"] + "api/Account/GetRole/" + userId.Value);
                    IEnumerable<string> roles = JsonConvert.DeserializeObject<IEnumerable<string>>(content);
                    foreach (var role in roles)
                    {
                        ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(JwtClaimTypes.Role, role, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role%22"));
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
