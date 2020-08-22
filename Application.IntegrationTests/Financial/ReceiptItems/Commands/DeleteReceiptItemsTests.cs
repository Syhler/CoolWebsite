using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.DeleteReceiptItems;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.ReceiptItems.Commands
{
    using static Testing;
    
    public class DeleteReceiptItemsTests : FinancialTestBase
    {
        [Test]
        public async Task Handle_ValidId_ShouldDeleteEntity()
        {
            var project = await CreateFinancialProject();
            var receiptId = await CreateReceipt(project);
            
            var createCommand = new CreateReceiptItemCommand
            {
                ReceiptId = receiptId,
                Price = 2,
                ItemGroup = (int)ItemGroup.Essentials,
                Count = 22,
                Name = "das",
                UserIds = new List<string>
                {
                    User.Id,
                    SecondUser.Id
                }
            };

            var id = await SendAsync(createCommand);

            var entityBeforeDeletion = await FindAsync<ReceiptItem>(id);

            entityBeforeDeletion.Should().NotBeNull();
            
            var deleteCommand = new DeleteReceiptItemCommand
            {
                Id = id,
                FinancialProjectId = project
            };
            var context = CreateContext();
            
            var oweRecordBeforeDeleted = context.OweRecords.First(x => x.UserId == SecondUser.Id && x.OwedUserId == User.Id);
            oweRecordBeforeDeleted.Amount.Should()
                .Be(createCommand.Price * createCommand.Count / createCommand.UserIds.Count);
            
            await SendAsync(deleteCommand);
            
            context = CreateContext();
            
            var entity = await FindAsync<ReceiptItem>(id);

            entity.Should().BeNull();

            var oweRecord = context.OweRecords.First(x => x.UserId == SecondUser.Id && x.OwedUserId == User.Id);
            oweRecord.Amount.Should().Be(0);

        }

        [Test]
        public void Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var command = new DeleteReceiptItemCommand
            {
                Id = "nah",
                FinancialProjectId = "ada"

            };

            FluentActions.Awaiting(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void Handle_IdEmpty_ShouldThrowValidationException()
        {
            var command = new DeleteReceiptItemCommand
            {
                Id = "",
                FinancialProjectId = "ada"
            };

            FluentActions.Awaiting(() => SendAsync(command)).Should().Throw<ValidationException>();
        }
        
        [Test]
        public void Handle_FinancialProjectIdEmpty_ShouldThrowValidationException()
        {
            var command = new DeleteReceiptItemCommand
            {
                Id = "asdas",
                FinancialProjectId = ""
            };

            FluentActions.Awaiting(() => SendAsync(command)).Should().Throw<ValidationException>();
        }
        
        [Test]
        public void Handle_FinancialProjectIdNull_ShouldThrowValidationException()
        {
            var command = new DeleteReceiptItemCommand
            {
                Id = "asdas",
                FinancialProjectId = null!
            };

            FluentActions.Awaiting(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        
        
    }
}