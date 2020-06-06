using System;
using System.Collections.Generic;

namespace CoolWebsite.Areas.Financial.Models
{
    public class ReceiptItemModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public string Type { get; set; }

        public List<UserModel> Users { get; set; }
        
    }
}