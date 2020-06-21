using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class UserDto : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, UserDto>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.FirstName+ " "+ x.LastName));
        }
    }
}