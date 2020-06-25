using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Domain.Enums;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class Transaction
    {
        public string Id { get; set; }

        public string FromUserId { get; set; }

        public ApplicationUser FromUser { get; set; }
        
        public string ToUserId { get; set; }
        
        public ApplicationUser ToUser { get; set; }
        
        public double Amount { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}