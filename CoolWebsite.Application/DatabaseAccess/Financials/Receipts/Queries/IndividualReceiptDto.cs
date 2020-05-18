using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries
{
    public class IndividualReceiptDto : IMapFrom<IndividualReceipt>
    {

        public string Id { get; set; }

        public double Total { get; set; }

        public ApplicationUser User { get; set; }

        public ReceiptsDto Receipt { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<IndividualReceipt, IndividualReceiptDto>()
                .ForMember(x => x.User, memberOptions
                    => memberOptions.MapFrom(x => x.User))
                .ForMember(x => x.Receipt, opt => 
                    opt.MapFrom(x => x.Receipt));
        }
    }
}