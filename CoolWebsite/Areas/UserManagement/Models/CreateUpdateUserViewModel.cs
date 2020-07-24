using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.UserManagement.Models
{
    public class CreateUpdateUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string RoleName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
        
        public List<SelectListItem>? Roles { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, CreateUpdateUserViewModel>();
            profile.CreateMap<CreateUpdateUserViewModel, UpdateApplicationUser>();
        }
    }
}