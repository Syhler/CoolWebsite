using System;

namespace CoolWebsite.Application.Common.Exceptions
{
    public class ParentObjectNotFoundException: Exception
    {
        public ParentObjectNotFoundException(string name, object key) :
            base($"Parent Entity \"{name}\" ({key}) not found ")
        {
            
        }
    }
}