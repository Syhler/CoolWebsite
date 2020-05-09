using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Domain.Entities.Identity;
using Shouldly;
using Xunit;
using CurrentUserService = CoolWebsite.UnitTest.Services.CurrentUserService;

namespace CoolWebsite.UnitTest.database.Financial.FinancialProject.Commands
{
    public class CreateFinancialProjectCommandTest : CommandTestBase
    {
        [Fact]
        public async Task Handle_Create_Succeed()
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = "test@gmail.com"
            };
            
            var createCommand = new CreateFinancialProjectCommand
            {
                Name = "First Project",
                Users = new List<ApplicationUser>
                {
                   user
                }
            };
            var createHandler = 
                new CreateFinancialProjectCommandHandler(Context,new CurrentUserService());

            var result = await createHandler.Handle(createCommand, CancellationToken.None);

            var entity = await Context.FinancialProjects.FindAsync(result);
            
            
            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(createCommand.Name);
            entity.FinancialProjectApplicationUsers.First().UserId.ShouldBe(user.Id);
        }
        
    }
}