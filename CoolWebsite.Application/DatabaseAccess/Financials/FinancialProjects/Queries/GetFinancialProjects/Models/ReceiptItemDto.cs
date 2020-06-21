using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Enums;
using CoolWebsite.Domain.Entities.Financial;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class ReceiptItemDto : IMapFrom<ReceiptItem>
    {
        public string Id { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        
        public ItemGroup ItemGroup { get; set; }
        public ICollection<ApplicationUserReceiptItem> Users { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReceiptItem, ReceiptItemDto>();
        }
    }
}