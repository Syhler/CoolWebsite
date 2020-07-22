using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class UserDto : IMapFrom<ApplicationUser>
    {
        public string? Id { get; set; }
        public string? Name { get; set; }

        public double Owed { get; set; }
        
        public string Initial
        {
            get
            {
                var split = Name?.Split(" ");
                var initial = "";
                
                if (split == null) return initial;
                
                foreach (var s in split)
                {
                    initial += s.ToUpper()[0];
                }

                return initial;
            }
        }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, UserDto>()
                .ForMember(x => x.Name, opt =>
                    opt.MapFrom(x => x.FirstName + " " + x.LastName));
            
            
        }
    }
}