using System.Security.Claims;
using System.Security.Principal;

namespace CoolWebsite.Services
{
    public static class UserExtension
    {
        
        public static string GetFirstName(this IPrincipal user)
        {
            if (user?.Identity == null) return "";
            
            var firstName = ((ClaimsIdentity)user.Identity).FindFirst("FirstName");
            
            return firstName != null ? firstName.Value : "";
        }

        public static string GetLastName(this IPrincipal user)
        {
            if (user?.Identity == null) return "";
            
            var lastName = ((ClaimsIdentity)user.Identity).FindFirst("LastName");
            
            return lastName != null ? lastName.Value : "";
        }
    }
}