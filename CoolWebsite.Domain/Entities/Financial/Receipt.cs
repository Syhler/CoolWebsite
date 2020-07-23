using System;
using System.Collections.Generic;
using CoolWebsite.Domain.Common;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class Receipt : AudibleEntity
    {
        public string Id { get; set; }

        public string Location { get; set; }
        
        public string Note { get; set; }
        
        public DateTime DateVisited { get; set; }

        public DateTime? Deleted { get; set; }
        
        public string FinancialProjectId { get; set; }
        
        public FinancialProject FinancialProject { get; set; }

        public ICollection<ReceiptItem> Items { get; set; }
        
        public string DeletedByUserId { get; set; }

        public ApplicationUser DeletedByUser { get; set; }
        
    }
}