using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.DeleteReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.DeleteReceipts;
using CoolWebsite.Domain.Entities.Financial;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.Receipts.Commands
{
    using static Testing;
    
    public class DeleteReceiptsTests : FinancialTestBase
    {

        [Test]
        public async Task Handle_ValidID_ShouldDelete()
        {
            
            var id = await CreateReceipt();

            var command = new DeleteReceiptsCommand
            {
                Id = id
            };

            var notDeleted = await FindAsync<Receipt>(command.Id);

            notDeleted.Should().NotBeNull();
            
            await SendAsync(command);

            var entity = await FindAsync<Receipt>(command.Id);

            entity.Deleted.Should().BeCloseTo(DateTime.Now, 1000);
        }

        [Test]
        public async Task Handle_Invalid_ShouldThrowNotFoundException()
        {
            
            var command = new DeleteReceiptsCommand
            {
                Id = "asdasd"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task Handle_IdEmpty_ShouldThrowValidationException()
        {
            var command = new DeleteReceiptsCommand();

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}