using System.Security.Claims;
using System.Security.Principal;

namespace CoolWebsite.Services
{
    public static class UserExtension
    {
        
        public static string GetFirstName(this IPrincipal user)
        {
            return GetClaim(user, "FirstName");
        }

        public static string GetLastName(this IPrincipal user)
        {
            return GetClaim(user, "LastName");
        }

        public static string GetUserId(this IPrincipal user)
        {
            return GetClaim(user, "UserId");
        }

        private static string GetClaim(IPrincipal user,string type)
        {
            if (user?.Identity == null) return "";
            
            var claim = ((ClaimsIdentity)user.Identity).FindFirst(type);
            
            return claim != null ? claim.Value : "";
        }
    }
}