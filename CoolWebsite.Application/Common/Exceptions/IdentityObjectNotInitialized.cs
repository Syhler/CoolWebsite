using System;

namespace CoolWebsite.Application.Common.Exceptions
{
    public class IdentityObjectNotInitialized : Exception
    {
        public IdentityObjectNotInitialized(string identity) : 
            base("Identity was not initialized: " + identity)
        {
            
        }
    }
}