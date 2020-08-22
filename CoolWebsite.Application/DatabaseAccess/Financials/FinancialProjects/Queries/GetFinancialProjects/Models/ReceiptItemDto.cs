using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries.Models;
using CoolWebsite.Domain.Entities.Financial;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class ReceiptItemDto : IMapFrom<ReceiptItem>
    {
        public string? Id { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }

        public string? Name { get; set; }
        
        public ItemGroupDto ItemGroup { get; set; } = null!;
        public ICollection<UserDto> Users { get; set; } = null!;

        public double Total => Price * Count;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReceiptItem, ReceiptItemDto>()
                .ForMember(x => x.Users,
                    opt => opt.MapFrom(x => x.Users.Select(y => y.ApplicationUser)))
                .ForMember(x => x.ItemGroup, opt => 
                    opt.MapFrom(y => new ItemGroupDto {Value = (int) y.ItemGroup, Name = y.ItemGroup.ToString()}));
        }
    }
}