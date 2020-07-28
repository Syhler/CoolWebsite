using System.Collections.Generic;
using CoolWebsite.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Models
{
    public class CreateFinancialProjectModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<string> Users { get; set; } = null!;

        public AddUserModel AddUserModel { get; set; } = null!;
    }
}