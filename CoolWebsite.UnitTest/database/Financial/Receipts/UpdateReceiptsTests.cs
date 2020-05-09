using System;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.UpdateReceipts;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Infrastructure.Services;
using CoolWebsite.UnitTest.Services;
using Shouldly;
using Xunit;
using CurrentUserService = CoolWebsite.UnitTest.Services.CurrentUserService;

namespace CoolWebsite.UnitTest.database.Financial.Receipts
{
    public class UpdateReceiptsTests : CommandTestBase
    {

        [Fact]
        public async Task Handle_ValidId_SuccessfullyUpdate()
        {
            var financialProjectId = await CreateFinancialProject();

            var id = Guid.NewGuid().ToString();
            
            await Context.Receipts.AddAsync(new Receipt
            {
                Total = 100,
                FinancialProjectId = financialProjectId,
                Id = id
            });
            
            var command = new UpdateReceiptsCommand
            {
                Id = id,
                FinancialProjectId = financialProjectId,
                Total = 1000
            };
            
            var handler = new UpdateReceiptsCommandHandler(Context, new CurrentUserService());

            await handler.Handle(command, CancellationToken.None);

            var entity = await Context.Receipts.FindAsync(command.Id);
            
            entity.ShouldNotBeNull();
            entity.FinancialProjectId.ShouldBe(command.FinancialProjectId);
            entity.Total.ShouldBe(command.Total);

        }

        [Fact]
        public async Task Handle_InvalidId_ThrowNotFoundException()
        {
            var financialProjectId = await CreateFinancialProject();

            var command = new UpdateReceiptsCommand
            {
                Id = "asd",
                FinancialProjectId = financialProjectId,
                Total = 100
            };
            
            var handler = new UpdateReceiptsCommandHandler(Context, new CurrentUserService());

            Should.Throw<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}