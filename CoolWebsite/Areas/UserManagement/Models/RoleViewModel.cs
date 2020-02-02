using System.ComponentModel.DataAnnotations;

namespace CoolWebsite.Areas.UserManagement.Models
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; }
        
    }
}