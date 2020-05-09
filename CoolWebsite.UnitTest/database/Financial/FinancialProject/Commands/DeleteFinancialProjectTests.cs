using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.DeleteFinancialProject;
using CoolWebsite.UnitTest.Services;
using Shouldly;
using Xunit;

namespace CoolWebsite.UnitTest.database.Financial.FinancialProject.Commands
{
    public class DeleteFinancialProjectCommandTest : CommandTestBase
    {

        //TODO(WHAT ABOUT IF PROJECT CONTAINS RECIEPTS)
        
        [Fact]
        public async Task Handle_ValidId_ShouldDelete()
        {
            var createCommand = new CreateFinancialProjectCommand
            {
                Name = "Delete me"
            };
            
            var createHandler = new CreateFinancialProjectCommandHandler(Context, new CurrentUserService());

            var id = await createHandler.Handle(createCommand, CancellationToken.None);

            
            var command = new DeleteFinancialProjectCommand
            {
                Id = id
            };
            
            var deleteHandler = new DeleteFinancialProjectCommandHandler(Context, new CurrentUserService());

            await deleteHandler.Handle(command, CancellationToken.None);
            
            var entity = await Context.FinancialProjects.FindAsync(id);
            
            entity.ShouldBeNull();
        }

        [Fact]
        public async Task Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var command = new DeleteFinancialProjectCommand
            {
                Id = "nah"
            };
            
            var deleteHandler = new DeleteFinancialProjectCommandHandler(Context, new CurrentUserService());

            Should.Throw<NotFoundException>(() => deleteHandler.Handle(command, CancellationToken.None));

        }
    }
}