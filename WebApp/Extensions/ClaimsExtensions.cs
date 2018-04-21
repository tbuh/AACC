using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebApp
{
    public static class ClaimsExtensions
    {
        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "Admin") != null;
        }

        public static bool IsSuperAdmin(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "SuperAdmin") != null;
        }
    }
}
