using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Queries.GetFinancialProjects;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.UnitTest.Services;
using Xunit;

namespace CoolWebsite.UnitTest.database.Financial.FinancialProject.Queries
{
    public class GetFinancialProjectsByUserTest : CommandTestBase
    {
        [Fact]
        public async Task Handle_ValidUser_ShouldReturnObject()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString()
            };
            
            var command = new CreateFinancialProjectCommand
            {
                Name = "test",
                Users = new List<ApplicationUser>
                {
                    user
                }
            };
            
            var handler = new CreateFinancialProjectCommandHandler(Context, new CurrentUserService());

            var id = await handler.Handle(command, CancellationToken.None);

            var query = new GetFinancialProjectsByUserQuery
            {
                UserId = user.Id
            };
            
            var queryHandler = new GetFinancialProjectsByUserQueryHandler(Context, null, new CurrentUserService());

        }
    }
}