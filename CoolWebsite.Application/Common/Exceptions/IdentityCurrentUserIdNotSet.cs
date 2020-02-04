using System;

namespace CoolWebsite.Application.Common.Exceptions
{
    public class IdentityCurrentUserIdNotSet : Exception
    {
        public IdentityCurrentUserIdNotSet() : base("Current UserID was not set")
        {
            
        }
    }
}