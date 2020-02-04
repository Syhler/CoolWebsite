using System;
using CoolWebsite.Infrastructure.Persistence;
using Xunit;

namespace CoolWebsite.UnitTest
{
    public class CommandTestBase
    {
        public ApplicationDbContext Context { get;}

        public CommandTestBase()
        {
            Context = ApplicationDbFactory.Create();
        }

        public void Dispose()
        {
            ApplicationDbFactory.Destroy(Context);
        }
    }
}