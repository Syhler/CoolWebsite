using System;
using System.Collections.Generic;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Queries;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries;
using CoolWebsite.Domain.Entities.Financial;

namespace CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects
{
    public class ReceiptsDto : IMapFrom<Receipt>
    {
        public string Id { get; set; }
        public double Total { get; set; }

        public String Title { get; set; }

        public DateTime BoughtAt { get; set; }

        public IList<IndividualReceiptDto> IndividualReceipts { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Receipt, ReceiptsDto>()
                .ForMember(x => x.IndividualReceipts, opt =>
                    opt.MapFrom(x => x.Receptors));
        }
    }
}