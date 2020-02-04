using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Areas.UserManagement.Models
{
    public class RoleViewModel : IMapFrom<ApplicationRole>
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public void Mapping(Profile profile)
        {


            profile.CreateMap<ApplicationRole, RoleViewModel>()
                .ForMember(x => x.Id,
                    opt => opt.MapFrom(s => s.Id))
                .ForMember(n => n.Name,
                    memberOptions
                        => memberOptions.MapFrom(d => d.Name));
        }
    }
}