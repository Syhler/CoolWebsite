using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Models
{
    public class CreateReceiptItemVm
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public string? Type { get; set; }

        public List<SelectListItem>? TypesSelectListItems { get; set; }

        public AddUserModel? AddUserModel { get; set; }
    }
}