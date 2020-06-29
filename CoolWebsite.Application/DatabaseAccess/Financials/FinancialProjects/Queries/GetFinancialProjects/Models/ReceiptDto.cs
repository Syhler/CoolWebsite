using System;
using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Financial;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class ReceiptDto : IMapFrom<Receipt>
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public DateTime DateVisited { get; set; }

        public string Note { get; set; }

        public IList<ReceiptItemDto> Items{ get; set; }

        public DateTime? Deleted { get; set; }

        public string CreatedByUserId { get; set; }

        public UserDto CreatedByDto { get; set; }
        
        public double Total
        {
            get
            {
                double total = 0;

                foreach (var item in Items)
                {
                    total += item.Price * item.Count;
                }
                
                return total;
            }
            private set
            {
                
            }
        }

        public int DaysSinceLastVisit
        {
            get
            {
                return (DateTime.Now - DateVisited).Days;
            }
            private set
            {
                
            }
        }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Receipt, ReceiptDto>()
                .ForMember(x => x.Items,
                    opt => opt.MapFrom(x => x.Items))
                .ForMember(x => x.CreatedByDto, 
                    opt => opt.MapFrom(x => x.CreatedByUser));
        }
    }
}