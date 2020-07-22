using System.Collections.Generic;

namespace CoolWebsite.Areas.UserManagement.Models
{
    public class UsersViewModel
    {
        public List<UserModel>? Users { get; set; }
        public CreateUpdateUserViewModel? CreateUpdateUserViewModel { get; set; }
    }
}