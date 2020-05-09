using System;
using System.Security.Claims;
using CoolWebsite.Application.Common.Interfaces;

namespace CoolWebsite.UnitTest.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService()
        {
            UserID = Guid.Empty.ToString();
        }
        public string UserID { get; set; }
    }
}