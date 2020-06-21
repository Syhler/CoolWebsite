using System.Collections.Generic;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.DeleteReceiptItems;
using CoolWebsite.Domain.Entities.Enums;
using CoolWebsite.Domain.Entities.Financial;
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
            var receiptId = await CreateReceipt();
            
            var createCommand = new CreateReceiptItemCommand
            {
                ReceiptId = receiptId,
                Price = 2,
                ItemGroup = (int)ItemGroup.Essentials,
                Count = 22,
                Name = "das",
                UsersId = new List<string>
                {
                    User.Id
                }
            };

            var id = await SendAsync(createCommand);

            var entityBeforeDeletion = await FindAsync<ReceiptItem>(id);

            entityBeforeDeletion.Should().NotBeNull();
            
            var deleteCommand = new DeleteReceiptItemCommand
            {
                ReceiptId = id
            };

            await SendAsync(deleteCommand);

            var entity = await FindAsync<ReceiptItem>(id);

            entity.Should().BeNull();

        }

        [Test]
        public void Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var command = new DeleteReceiptItemCommand
            {
                ReceiptId = "nah"
            };

            FluentActions.Awaiting(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void Handle_IdEmpty_ShouldThrowValidationException()
        {
            var command = new DeleteReceiptItemCommand
            {
                ReceiptId = ""
            };

            FluentActions.Awaiting(() => SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}