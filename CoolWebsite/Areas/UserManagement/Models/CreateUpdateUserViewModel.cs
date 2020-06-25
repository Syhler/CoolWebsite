using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.UserManagement.Models
{
    public class CreateUpdateUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public List<SelectListItem> Roles { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, CreateUpdateUserViewModel>();
            profile.CreateMap<CreateUpdateUserViewModel, UpdateApplicationUser>();
        }
    }
}