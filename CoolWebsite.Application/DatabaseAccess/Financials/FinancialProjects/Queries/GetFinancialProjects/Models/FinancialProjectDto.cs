﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models
{
    public class FinancialProjectDto : IMapFrom<Domain.Entities.Financial.FinancialProject>
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public IList<UserDto> Users { get; set; }
        public IList<ReceiptDto> Receipts { get; set; }

        public DateTime? LastModified { get; set; }

        public DateTime Created { get; set; }

     

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Financial.FinancialProject, FinancialProjectDto>()
                .ForMember(x => x.Title, opt => opt.MapFrom(s => s.Title))
                .ForMember(x => x.LastModified, opt =>
                    opt.MapFrom(x => x.LastModified))
                .ForMember(x => x.Users, opt => 
                    opt.MapFrom(x => x.FinancialProjectApplicationUsers.Select(y => y.User)));
        }
    }
}