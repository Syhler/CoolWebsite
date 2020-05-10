using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Infrastructure.Persistence;
using CoolWebsite.Infrastructure.Services;
using CoolWebsite.UnitTest.Services;
using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using CurrentUserService = CoolWebsite.UnitTest.Services.CurrentUserService;

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

        public async Task<string> CreateFinancialProject()
        {
            var command = new CreateFinancialProjectCommand
            {
                Title = "test",
                Users = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString()
                    }
                }
            };
            
            var handler = new CreateFinancialProjectCommandHandler(Context, new CurrentUserService());

            var id = await handler.Handle(command, CancellationToken.None);

            return id;
        }
    }
}