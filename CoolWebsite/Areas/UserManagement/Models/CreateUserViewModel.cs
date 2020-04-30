using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoolWebsite.Areas.UserManagement.Models
{
    public class CreateUserViewModel 
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}