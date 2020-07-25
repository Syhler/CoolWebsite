using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Enums;

namespace CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries.Models
{
    public class TransactionTypeDto : IMapFrom<TransactionType>
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TransactionType, TransactionTypeDto>()
                .ForMember(x => x.Name,
                    opt => opt.MapFrom(x => x.ToString()))
                .ForMember(x => x.Value,
                    opt => opt.MapFrom(x => (int) x));
            
        }
    }
}