using System;
using System.Collections.Generic;
using CoolWebsite.Domain.Common;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class Receipt : AudibleEntity
    {
        public string Id { get; set; }

        public string Title { get; set; }
        
        public double Total { get; set; }

        public DateTime BoughtAt { get; set; }
        
        public ICollection<IndividualReceipt> Receptors { get; set; }
        
        public string FinancialProjectId { get; set; }
        public FinancialProject FinancialProject { get; set; }
    }
}