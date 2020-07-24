using System;
using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Financial;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class ReceiptDto : IMapFrom<Receipt>
    {
        public string Id { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime DateVisited { get; set; }

        public string? Note { get; set; }

        public IList<ReceiptItemDto> Items{ get; set; } = null!;

        public FinancialProjectDto FinancialProject { get; set; } = null!;
        public DateTime? Deleted { get; set; }

        public string CreatedByUserId { get; set; } = null!;

        public UserDto CreatedByDto { get; set; } = null!;
        
        public double Total
        {
            get
            {
                double total = 0;
                if (Items == null)
                {
                    return total;
                }
                foreach (var item in Items)
                {
                    total += item.Price * item.Count;
                }

                return total;
            }
        }

        public int DaysSinceLastVisit => (DateTime.Now - DateVisited).Days;

        public int DaysSinceDeleted => (DateTime.Now - Deleted.GetValueOrDefault()).Days;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Receipt, ReceiptDto>()
                .ForMember(x => x.Items,
                    opt => opt.MapFrom(x => x.Items))
                .ForMember(x => x.FinancialProject,
                    opt => opt.MapFrom(x => x.FinancialProject))
                .ForMember(x => x.CreatedByDto, 
                    opt => opt.MapFrom(x => x.CreatedByUser));
        }
    }
}