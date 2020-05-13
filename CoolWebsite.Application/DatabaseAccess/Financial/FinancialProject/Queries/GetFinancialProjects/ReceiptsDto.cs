using System;
using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Domain.Entities.Financial;

namespace CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Queries.GetFinancialProjects
{
    public class ReceiptsDto : IMapFrom<Receipt>
    {
        public string Id { get; set; }
        public double Total { get; set; }

        public String Title { get; set; }

        public DateTime BoughtAt { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Receipt, ReceiptsDto>();
        }
    }
}