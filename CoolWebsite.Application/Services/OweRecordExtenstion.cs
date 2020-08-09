using System;
using System.Collections.Generic;
using System.Linq;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Domain.Entities.Financial;

namespace CoolWebsite.Application.Services
{
    public static class OweRecordExtenstion
    {

        public static void AddReceiptItemCost(this ICollection<OweRecord> records, List<string> userIds, double price,
            double count)
        {
            foreach (var userId in userIds)
            {
                var record = records.FirstOrDefault(x => x.UserId == userId);

                if (record == null) continue;

                record.Amount += Math.Round(count * price / userIds.Count, 2);
            }
        }

        public static void SubtractReceiptItemCost(this ICollection<OweRecord> records, List<string> userIds,
            double price, double count, int userCount)
        {
            foreach (var userId in userIds)
            {
                var record = records.FirstOrDefault(x => x.UserId == userId);

                if (record == null) continue;

                record.Amount -= Math.Round(count * price / userCount, 2);
            }
        }
    }
}