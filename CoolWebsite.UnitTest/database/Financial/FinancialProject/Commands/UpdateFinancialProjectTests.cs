using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.UpdateFinancialProject;
using Shouldly;
using Xunit;
using CurrentUserService = CoolWebsite.UnitTest.Services.CurrentUserService;

namespace CoolWebsite.UnitTest.database.Financial.FinancialProject.Commands
{
    public class UpdateFinancialProjectCommandTest : CommandTestBase
    {

        [Fact]
        public async Task Handle_ValidId_ShouldUpdateName()
        {
            var createCommand = new CreateFinancialProjectCommand
            {
                Name = "First Project"
            };
            
            var createHandler = new CreateFinancialProjectCommandHandler(Context, new CurrentUserService());

            var id = await createHandler.Handle(createCommand, CancellationToken.None);
            
            var updateCommand = new UpdateFinancialProjectCommand
            {
                Name = "First Project With new name",
                Id = id
            };
            
            var updateHandler = new UpdateFinancialProjectCommand.UpdateFinancialProjectCommandHandler(Context, new CurrentUserService());

            await updateHandler.Handle(updateCommand, CancellationToken.None);

            var entity = await Context.FinancialProjects.FindAsync(id);
            
            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(updateCommand.Name);
        }

        [Fact]
        public async Task Handle_InvalidId_ShouldThrowNotFoundExpecption()
        {
            var command = new UpdateFinancialProjectCommand
            {
                Id = "doesnt exsist",
                Name = "asdasdas"
            };
            
            var handler = new UpdateFinancialProjectCommand.UpdateFinancialProjectCommandHandler(Context, new CurrentUserService());

            Should.Throw<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}