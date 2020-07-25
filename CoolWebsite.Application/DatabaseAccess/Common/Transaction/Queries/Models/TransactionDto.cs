using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries.Models
{
    public class TransactionDto : IMapFrom<Domain.Entities.Financial.Transaction>
    {
        public string Id { get; set; }

        public UserDto FromUser { get; set; }

        public UserDto ToUser { get; set; }

        public double Amount { get; set; }

        public TransactionTypeDto TransactionTypeDto { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Financial.Transaction, TransactionDto>()
                .ForMember(x => x.FromUser,
                    opt => opt.MapFrom(x => x.FromUser))
                .ForMember(x => x.ToUser,
                    opt => opt.MapFrom(x => x.ToUser))
                .ForMember(x => x.TransactionTypeDto,
                    opt => opt.MapFrom(x => x.TransactionType));
            
        }
    }
}