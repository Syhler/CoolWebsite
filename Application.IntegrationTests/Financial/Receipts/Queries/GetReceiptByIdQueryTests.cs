using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
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
            var receiptId = await CreateReceipt();
            //var user = await RunAsDefaultUserAsync();

            
            var query = new GetReceiptByIdQueryVm
            {
                ReceiptId = receiptId
            };

            var model = await SendAsync(query);


            model.Should().NotBeNull();
           

        }
        
        //notfoundexception
        //emptyid
        
    }
}