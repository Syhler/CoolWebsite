using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Application.Common.Dto
{
    public class ApplicationUserDto : IMapFrom<ApplicationUser>
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        
    }
}