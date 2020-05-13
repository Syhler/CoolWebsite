using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.CreateIndividualReceipt;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Queries;
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
            var receiptId = await CreateReceipt();
            var user = await RunAsDefaultUserAsync();

            await AddAsync(user);
            
            var createCommand = new CreateIndividualReceiptCommand
            {
                Total = 12312.123,
                ReceiptId = receiptId,
                UserId = user.Id
            };

            var id = await SendAsync(createCommand);
            
            var query = new GetReceiptByIdQueryVm
            {
                ReceiptId = receiptId
            };

            var model = await SendAsync(query);


            model.Should().NotBeNull();
            model.IndividualReceipts.Should().NotBeNull();
            model.IndividualReceipts.First().Receipt.Should().NotBeNull();
            model.IndividualReceipts.First().User.Should().NotBeNull();
            model.IndividualReceipts.First().Id.Should().Be(id);
            model.IndividualReceipts.First().Receipt.Id.Should().Be(createCommand.ReceiptId);
            model.IndividualReceipts.First().Total.Should().Be(createCommand.Total);
            model.IndividualReceipts.First().User.Id.Should().Be(user.Id);

        }
        
        //notfoundexception
        //emptyid
        
    }
}