using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.ReceiptItems.Queries
{
    using static Testing;
    
    public class GetReceiptItemsByReceiptIdQueryTests : FinancialTestBase
    {

        [Test]
        public async Task Handle_ValidReceiptId_ShouldReturnItems()
        {
            var receipt = await CreateReceipt();

            var createReceiptItem = new CreateReceiptItemCommand
            {
                ItemGroup = 0,
                Count = 10,
                Name = "hey",
                Price = 1235,
                ReceiptId = receipt,
                UserIds = new List<string>
                {
                    User.Id
                }
            };

            var itemId = await SendAsync(createReceiptItem);
            
            var query = new GetReceiptItemByReceiptIdQuery{ReceiptId = receipt};

            var entities = await SendAsync(query);

            entities.Should().NotBeNull();
            entities.Count.Should().Be(1);
            entities.First().Id.Should().Be(itemId);
        }
        //succeed
        //notfound
        //empty
        //null
    }
}