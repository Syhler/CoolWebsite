using Microsoft.AspNetCore.Identity;

namespace CoolWebsite.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Color { get; set; }
    }
}