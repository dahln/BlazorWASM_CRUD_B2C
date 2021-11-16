using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BlazorWASM_CRUD_B2C.API.Utility
{
    public static class UserUtility
    {
        public static string GetUserId(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return claim.Value;
        }

        public static string GetUserEmail(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst("Emails");
            return claim.Value;
        }

        private static List<string> GetUserRoles(IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claims = claimsIdentity.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
            return claims;
        }
    }
}
