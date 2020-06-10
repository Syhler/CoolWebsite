using System;
using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Financial;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class ReceiptsDto : IMapFrom<Receipt>
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public DateTime DateVisited { get; set; }


        public ICollection<ReceiptItemDto> Item{ get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Receipt, ReceiptsDto>()
                .ForMember(x => x.Item, 
                    opt => opt.MapFrom(x => x.Items));
        }
    }
}