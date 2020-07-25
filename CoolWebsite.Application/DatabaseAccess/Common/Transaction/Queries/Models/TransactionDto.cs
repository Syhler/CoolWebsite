using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries.Models
{
    public class TransactionDto : IMapFrom<Domain.Entities.Financial.Transaction>
    {
        public string Id { get; set; } = null!;

        public UserDto FromUser { get; set; } = null!;

        public UserDto ToUser { get; set; } = null!;

        public double Amount { get; set; }

        public TransactionTypeDto TransactionTypeDto { get; set; } = null!;

        public string ProjectTitle { get; set; } = null!;
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Financial.Transaction, TransactionDto>()
                .ForMember(x => x.FromUser,
                    opt => opt.MapFrom(x => x.FromUser))
                .ForMember(x => x.ToUser,
                    opt => opt.MapFrom(x => x.ToUser))
                .ForMember(x => x.TransactionTypeDto,
                    opt => opt.MapFrom(x => x.TransactionType))
                .ForMember(x => x.ProjectTitle,
                    opt => opt.MapFrom(x => x.FinancialProject.Title));
            
        }
    }
}