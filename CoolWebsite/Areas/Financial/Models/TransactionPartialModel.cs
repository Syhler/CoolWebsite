using System.Collections.Generic;
using System.Linq;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries.Models;

namespace CoolWebsite.Areas.Financial.Models
{
    public class TransactionPartialModel
    {
        public List<TransactionDto>? PayedTransaction { get; set; }
        
        public List<TransactionDto>? ReceivedTransaction { get; set; }

        public double ReceivedTotal
        {
            get
            {
                return ReceivedTransaction.Sum(transactionDto => transactionDto.Amount);
            }
        }

        public double PayedTotal
        {
            get
            {
                return PayedTransaction.Sum(x => x.Amount);
            }
        }
    }
}