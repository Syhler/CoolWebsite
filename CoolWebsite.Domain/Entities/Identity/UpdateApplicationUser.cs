﻿namespace CoolWebsite.Domain.Entities.Identity
{
    public class UpdateApplicationUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }

        public string PhoneNumber { get; set; }
        public string Id { get; set; }
    }
}