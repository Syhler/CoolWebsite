using System.Collections.Generic;
using CoolWebsite.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Models
{
    public class CreateFinancialProjectModel
    {
        public string Name { get; set; }
        public List<string> Users { get; set; }
        public List<SelectListItem>  UsersDropdown { get; set; }
    }
}