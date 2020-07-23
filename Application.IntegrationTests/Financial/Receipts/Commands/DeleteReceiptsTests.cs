using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
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
        [TestCase(1)]
        [TestCase(2)]
        public async Task Handle_ValidID_ShouldDeleteAndUpdateOweRecord(int userAmount)
        {
            
            
            var projectId = await CreateFinancialProject();
            
            var id = await CreateReceipt(projectId);

            
            var createReceiptItemCommand = new CreateReceiptItemCommand
            {
                ItemGroup = 0,
                Count = 100,
                Name = "test",
                Price = 10,
                ReceiptId = id,
            };

            createReceiptItemCommand.UserIds = userAmount switch
            {
                1 => new List<string> {SecondUser.Id},
                2 => new List<string> {User.Id, SecondUser.Id},
                _ => createReceiptItemCommand.UserIds
            };

            await SendAsync(createReceiptItemCommand);
          
            var command = new DeleteReceiptCommand
            {
                Id = id
            };

            var context = CreateContext();

            var oweRecord = context.OweRecords.FirstOrDefault(x =>
                x.OwedUserId == User.Id && x.UserId == SecondUser.Id && x.FinancialProjectId == projectId);

            oweRecord.Should().NotBeNull();
            oweRecord.Amount.Should().Be(createReceiptItemCommand.Count * createReceiptItemCommand.Price / createReceiptItemCommand.UserIds.Count);
            
            var notDeleted = await FindAsync<Receipt>(command.Id);

            notDeleted.Should().NotBeNull();
            
            await SendAsync(command);

            var entity = await FindAsync<Receipt>(command.Id);

            context = CreateContext();
            
            var updatedOweRecord = context.OweRecords.FirstOrDefault(x =>
                x.OwedUserId == User.Id && x.UserId == SecondUser.Id && x.FinancialProjectId == projectId);

            entity.Should().NotBeNull();
            entity.Deleted.Should().BeCloseTo(DateTime.Now, 1000);
            entity.DeletedByUserId.Should().Be(User.Id);
            updatedOweRecord.Should().NotBeNull();
            updatedOweRecord.Amount.Should().Be(0);

        }

      

        [Test]
        public void Handle_Invalid_ShouldThrowNotFoundException()
        {
            
            var command = new DeleteReceiptCommand
            {
                Id = "asdasd"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void Handle_IdEmpty_ShouldThrowValidationException()
        {
            var command = new DeleteReceiptCommand();

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}