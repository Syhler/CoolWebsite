using System;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.DeleteReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Queries;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.Receipts.Queries
{
    using static Testing;
    
    public class GetArchiveReceiptsByUserQueryTests : FinancialTestBase
    {
        [Test]
        public async Task Handle_ValidUserId_ShouldReturnListOfReceipts()
        {
            var projectId = await CreateFinancialProject();
            
            var receiptId = await CreateReceipt(projectId);
            
            var deleteCommand = new DeleteReceiptCommand
            {
                Id = receiptId
            };

            await SendAsync(deleteCommand);

            var query = new GetArchiveReceiptsByUserQuery();

            var entities = await SendAsync(query);

            entities.Should().NotBeNull();
            entities.Count.Should().Be(1);
            entities.First().Id.Should().Be(receiptId);
            entities.First().FinancialProject.Id.Should().Be(projectId);
            entities.First().Deleted.Should().BeCloseTo(DateTime.Now, 1000);

        }
    }
}