using Microsoft.AspNetCore.Identity;

namespace CoolWebsite.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Color { get; set; }
    }
}