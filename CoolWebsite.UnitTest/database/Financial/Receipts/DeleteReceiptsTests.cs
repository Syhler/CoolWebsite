using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.DeleteReceipts;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Infrastructure.Services;
using CoolWebsite.UnitTest.Services;
using Shouldly;
using Xunit;
using CurrentUserService = CoolWebsite.UnitTest.Services.CurrentUserService;

namespace CoolWebsite.UnitTest.database.Financial.Receipts
{
    public class DeleteReceiptsTest : CommandTestBase
    {

        [Fact]
        public async Task Handle_ValidId_ShouldDelete()
        {
            var financialProjectId = await CreateFinancialProject();
            
            var command = new CreateReceiptsCommand
            {
                Total = 2123,
                FinancialProjectId = financialProjectId
            };
            
            var createHandler = new CreateReceiptsCommandHandler(Context, new CurrentUserService());

            var id = await createHandler.Handle(command, CancellationToken.None);


            var deleteCommand = new DeleteReceiptsCommand
            {
                Id = id
            };
            
            var deleteHandler = new DeleteReceiptsCommandHandler(Context, new CurrentUserService());

            await deleteHandler.Handle(deleteCommand, CancellationToken.None);

            var entity = await Context.Receipts.FindAsync(id);
            
            entity.ShouldBeNull();



        }

        [Fact]
        public async Task Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var deleteCommand = new DeleteReceiptsCommand
            {
                Id = "nah"
            };
            
            var deleteHandler = new DeleteReceiptsCommandHandler(Context, new CurrentUserService());

            Should.Throw<NotFoundException>(() => deleteHandler.Handle(deleteCommand, CancellationToken.None));
        }
    }
}