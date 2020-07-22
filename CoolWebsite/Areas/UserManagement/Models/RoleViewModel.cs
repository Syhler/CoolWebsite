using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Areas.UserManagement.Models
{
    public class RoleViewModel : IMapFrom<ApplicationRole>
    {
        public string Name { get; set; } = null!;

        public string Id { get; set; } = null!;

        public List<UserModel>? Users { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationRole, RoleViewModel>();
            
        }
    }
}