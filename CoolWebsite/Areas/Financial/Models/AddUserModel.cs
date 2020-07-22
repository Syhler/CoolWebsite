using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.Financial.Models
{
    public class AddUserModel
    {
        public List<SelectListItem>? UserSelectListItems { get; set; }
        public string? CurrentUserName { get; set; }
        public string? CurrentUserId { get; set; }
    }
}