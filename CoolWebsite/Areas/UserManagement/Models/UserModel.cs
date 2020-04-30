using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Areas.UserManagement.Models
{
    public class UserModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public List<string> RoleNames { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, UserModel>();
        }
    }
    
}