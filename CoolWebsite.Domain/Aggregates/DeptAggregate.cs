using System;
using System.Collections.Generic;
using System.Linq;
using CoolWebsite.Domain.Common.ValueObjects;

namespace CoolWebsite.Domain.Aggregates
{
    public class DeptAggregate
    {
        private string projectId;

        public DeptAggregate(string projectId)
        {
            this.projectId = projectId;
        }

        public List<Payment> Payments { get; set; }
        //public List<AmountOwed> AmountOwes { get; set; }

        public Dictionary<string, AmountOwed> AmountOwes { get; set; }

        public static DeptAggregate Load(string projectId)
        {
            return new DeptAggregate(projectId);
        }

        public void PayDebt(Payment payment)
        {
            //Send a command
            
            if (payment.ProjectId != projectId) throw new Exception();

            //Get payment amount
            var paymentAmount = payment.Amount;
            //Get How much this person owes
            var personDebtAmount = AmountOwes.Where(x =>
                x.Value.UserId == payment.UserId &&
                x.Value.ProjectId == payment.ProjectId &&
                x.Key == payment.ToUserId)
                .Sum(x => x.Value.Amount);
            //If payment > debt throw
            if (paymentAmount > personDebtAmount) throw new Exception();
            //Else
            Payments.Add(payment);
            //Add payment to payments
            
            //Complete payment
        }

        public void AddDebt(AmountOwed amountOwed, string toUserId)
        {
            AmountOwes.Add(toUserId, amountOwed);
        }

        public void RemovePayment(Payment payment)
        {
            
        }

        public void RemoveDebt(AmountOwed amountOwed, string toUserId)
        {
            
        }
        
        
        
    }
}