using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CoolWebsite.Domain.Common;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class FinancialProject : AudibleEntity
    {
        public string Id { get; set; }

        public string Title { get; set; }
        
        public ICollection<Receipt> Receipts { get; set; }

        public ICollection<FinancialProjectApplicationUser> FinancialProjectApplicationUsers { get; set; }

    }
}