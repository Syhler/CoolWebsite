using System.Collections;
using System.Collections.Generic;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries;
using CoolWebsite.Domain.Entities.Financial;

namespace CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Queries
{
    public class ReceiptVm
    {
        public IList<IndividualReceiptDto> IndividualReceipts { get; set; }
        
        
    }
}