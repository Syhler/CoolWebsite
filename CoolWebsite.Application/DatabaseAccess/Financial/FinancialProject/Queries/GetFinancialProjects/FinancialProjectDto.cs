using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Queries.GetFinancialProjects
{
    public class FinancialProjectDto : IMapFrom<Domain.Entities.Financial.FinancialProject>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IList<ApplicationUser> Users { get; set; }
        public IList<ReceiptsDto> Receipts { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Financial.FinancialProject, FinancialProjectDto>()
                .ForMember(x => x.Title, opt => opt.MapFrom(s => s.Title))
                .ForMember(x => x.Users, opt => 
                    opt.MapFrom(s => s.FinancialProjectApplicationUsers.Select(x => x.User)));
        }
    }
}