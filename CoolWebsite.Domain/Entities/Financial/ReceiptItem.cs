﻿using System;
using System.Collections.Generic;
using CoolWebsite.Domain.Common;
using CoolWebsite.Domain.Enums;

namespace CoolWebsite.Domain.Entities.Financial
{
    public class ReceiptItem : AudibleEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }

        public string ReceiptId { get; set; }
        
        public Receipt Receipt { get; set; }

        public ItemGroup ItemGroup { get; set; }
        
        public ICollection<ApplicationUserReceiptItem> Users { get; set; }

        
    }
}