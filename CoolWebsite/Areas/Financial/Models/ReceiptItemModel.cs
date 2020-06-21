using System;
using System.Collections.Generic;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries.Models;

namespace CoolWebsite.Areas.Financial.Models
{
    public class ReceiptItemModel
    {
        public double Price { get; set; }
        public int Count { get; set; }
        public ItemGroupDto Type { get; set; }

        public List<UserModel> Users { get; set; }

        public Guid UniqueIdentifier { get; set; }
        
    }
}