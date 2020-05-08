using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;

namespace Api.Services.RolesAndClaims
{
    public class KarekeClaimsTransformer : IClaimsTransformation
    {

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                Claim userId = principal.FindFirst("sub"); if (principal.FindFirst("role") == null && userId != null)
                {
                    using (var context = new LoveMirroringContext())
                    {
                        var roles = context.AspNetUserRoles.Where(a => a.UserId == userId.Value).Select(r => r.RoleId); foreach (var role in roles)
                        {
                            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(JwtClaimTypes.Role, role, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role%22"));
                        }
                    }
                }
            }
            return Task.FromResult(principal);
        }
    }

}
