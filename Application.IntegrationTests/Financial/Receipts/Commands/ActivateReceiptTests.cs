using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.ActivateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.DeleteReceipts;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.Receipts.Commands
{
    using static Testing;
    
    public class ActivateReceiptTests : FinancialTestBase
    {
        //success

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task Handle_ValidId_ShouldActivateReceipt(int amount)
        {
            var receiptId = await CreateReceipt();

            var item = GetReceiptItem(100);
            
            var command = new CreateReceiptItemCommand
            {
                Name = "dd",
                ItemGroup = item.ItemGroup.Value,
                Price = item.Price,
                Count = item.Count,
                ReceiptId = receiptId
               
            };

            command.UserIds = amount switch
            {
                1 => new List<string> {SecondUser.Id},
                2 => new List<string> {SecondUser.Id, User.Id},
                _ => command.UserIds
            };

            await SendAsync(command);

            var context = CreateContext();
            
            var firstOweRecord = context.OweRecords.FirstOrDefault(x => x.UserId == SecondUser.Id && x.OwedUserId == User.Id);
            firstOweRecord.Should().NotBeNull();
            firstOweRecord!.Amount.Should().Be(command.Count * command.Price / command.UserIds.Count);

            var deleteCommand = new DeleteReceiptCommand {Id = receiptId};

            await SendAsync(deleteCommand);

            context = CreateContext();

            var deletedEntity = context.Receipts.FirstOrDefault(x => x.Id == receiptId);
            deletedEntity.Should().NotBeNull();
            deletedEntity!.Deleted.Should().BeCloseTo(DateTime.Now, 1000);
            deletedEntity!.DeletedByUserId.Should().Be(User.Id);
             
            var secondOweRecord = context.OweRecords.FirstOrDefault(x => x.UserId == SecondUser.Id && x.OwedUserId == User.Id);
            secondOweRecord.Should().NotBeNull();
            secondOweRecord!.Amount.Should().Be(0);

            var reactivateCommand = new ActivateReceiptCommand{ReceiptId = receiptId};

            await SendAsync(reactivateCommand);

            context = CreateContext();
            
            var thirdOweRecord = context.OweRecords.FirstOrDefault(x => x.UserId == SecondUser.Id && x.OwedUserId == User.Id);
            thirdOweRecord.Should().NotBeNull();
            thirdOweRecord!.Amount.Should().Be(command.Count * command.Price / command.UserIds.Count);

            var activatedEntity = context.Receipts.FirstOrDefault(x => x.Id == receiptId);
            activatedEntity.Should().NotBeNull();
            activatedEntity!.Deleted.Should().BeNull();
            activatedEntity!.DeletedByUserId.Should().BeNull();

        }
        
        //validid but wrong user
        [Test]
        public async Task Handle_ValidIdButWrongUser_ShouldThrowNotFoundException()
        {
            var receiptId = await CreateReceipt();

            var delete = new DeleteReceiptCommand{Id = receiptId};
            await SendAsync(delete);
            
            await RunAsUserAsync("wronguser@u.com", "nah");

            var activate = new ActivateReceiptCommand{ReceiptId = receiptId};

            FluentActions.Invoking(async () => await SendAsync(activate)).Should().Throw<NotFoundException>();
        }
        
        //invalidid
        [Test]
        public void Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var command = new ActivateReceiptCommand
            {
                ReceiptId = "I DONT KNOW"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<NotFoundException>();
        }
        
        //empty
        [Test]
        public void Handle_IdIsEmpty_ShouldThrowValidationException()
        {
            var command = new ActivateReceiptCommand
            {
                ReceiptId = ""
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
        
        //null
        [Test]
        public void Handle_IdIsNull_ShouldThrowValidationException()
        {
            var command = new ActivateReceiptCommand
            {
                ReceiptId = null!
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}