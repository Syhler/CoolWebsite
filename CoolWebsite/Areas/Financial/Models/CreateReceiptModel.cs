using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Models
{
    public class CreateReceiptModel
    {
        public string Location { get; set; }
        
        public string Note { get; set; }
        
        public DateTime DateVisited { get; set; }

        public string FinancialProjectId { get; set; }

        public List<ReceiptItemModel> ReceiptItemModels { get; set; }

        //not sure
        public List<string> Users { get; set; }
        public List<SelectListItem>  UsersDropdown { get; set; }
    }
}