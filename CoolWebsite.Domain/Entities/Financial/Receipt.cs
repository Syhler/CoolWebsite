using System;
using System.Collections.Generic;
using CoolWebsite.Domain.Common;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class Receipt : AudibleEntity
    {
        public string Id { get; set; }

        public string Location { get; set; }
        
        //public double Total { get; set; }

        public string Note { get; set; }
        
        public DateTime DateVisited { get; set; }
        
        //public ICollection<IndividualReceiptObsolete> Receptors { get; set; }
        
        public string FinancialProjectId { get; set; }
        public FinancialProject FinancialProject { get; set; }

        public ICollection<ReceiptItem> Items { get; set; }
        
    }
}