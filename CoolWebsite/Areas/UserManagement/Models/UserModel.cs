using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Areas.UserManagement.Models
{
    public class UserModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public List<string> RoleNames { get; set; } = null!;
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, UserModel>();
        }
    }
    
}