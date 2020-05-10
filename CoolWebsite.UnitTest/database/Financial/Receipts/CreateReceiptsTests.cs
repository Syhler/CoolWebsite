using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.UnitTest.Services;
using Shouldly;
using Xunit;

namespace CoolWebsite.UnitTest.database.Financial.Receipts
{
    public class CreateReceiptsTest : CommandTestBase
    {
        
        [Fact]
        public async Task Handle_ShouldPersistReceipts()
        {
            var financialProjectId = await CreateFinancialProject();
            
            var command = new CreateReceiptsCommand
            {
                Total = 1000,
                FinancialProjectId = financialProjectId
            };
            
            var handler = new CreateReceiptsCommandHandler(Context, new CurrentUserService());

            var id = await handler.Handle(command, CancellationToken.None);

            var entity = await Context.Receipts.FindAsync(id);
            
            entity.ShouldNotBeNull();
            entity.Total.ShouldBe(command.Total);
            entity.FinancialProjectId.ShouldBe(command.FinancialProjectId);
        }
    }
}
