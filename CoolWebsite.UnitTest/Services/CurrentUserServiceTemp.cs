using System;
using CoolWebsite.Application.Common.Interfaces;

namespace CoolWebsite.UnitTest.Services
{
    public class CurrentUserServiceTemp : ICurrentUserService
    {
        public CurrentUserServiceTemp()
        {
            UserID = Guid.Empty.ToString();
        }
        public string UserID { get; set; }
    }
}