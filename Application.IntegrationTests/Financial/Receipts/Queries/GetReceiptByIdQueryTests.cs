using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.Receipts.Queries
{
    using static Testing;
    
    public class GetReceiptByIdQueryTests : FinancialTestBase
    {

        [Test]
        public async Task Handle_ValidReceiptId_ShouldReturnReceiptVm()
        {
            var financialProjectId = await CreateFinancialProject();
            
            var createCommand = new CreateReceiptCommand
            {
                FinancialProjectId = financialProjectId,
                Location = "Title",
                DateVisited = DateTime.Now,
                Note = "yooo a note"
            };

            var receiptId = await SendAsync(createCommand);

            var receiptItemCommand = new CreateReceiptItemCommand
            {
                Name = "idk",
                Count = 100,
                Price = 1000,
                ItemGroup = 0,
                ReceiptId = receiptId,
                UsersId = new List<string>
                {
                    User.Id,
                    SecondUser.Id
                }
                
            };

            var receiptItemId = await SendAsync(receiptItemCommand);
            
            
            var query = new GetReceiptByIdQuery
            {
                ReceiptId = receiptId
            };

            var model = await SendAsync(query);


            model.Should().NotBeNull();
            model.CreatedByUserId.Should().Be(User.Id);
            model.Location.Should().Be(createCommand.Location);
            model.Note.Should().Be(createCommand.Note);
            model.DateVisited.Should().BeCloseTo(DateTime.Now, 1000);
            model.CreatedByUserId.Should().Be(User.Id);
            model.Deleted.Should().BeNull();
            var item = model.Items.First();
            item.Should().NotBeNull();
            item.Id.Should().Be(receiptItemId);
            item.Count.Should().Be(receiptItemCommand.Count);
            item.Price.Should().Be(receiptItemCommand.Price);
            item.ItemGroup.Value.Should().Be(receiptItemCommand.ItemGroup);
            item.Users.Count.Should().Be(receiptItemCommand.UsersId.Count);


        }
        

        [Test]
        public void Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var query = new GetReceiptByIdQuery
            {
                ReceiptId = "asdasd"
            };

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void Handle_IdIsEmpty_ShouldThrowValidationException()
        {
            var query = new GetReceiptByIdQuery
            {
                ReceiptId = ""
            };

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_IdIsNull_ShouldThrowValidationException()
        {
            var query = new GetReceiptByIdQuery
            {
                ReceiptId = null
            };

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<ValidationException>();
        }
        
    }
}