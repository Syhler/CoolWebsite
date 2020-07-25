using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.CreateTransaction;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Transaction.Queries
{
    using static Testing;
    
    public class GetReceivedTransactionTests : FinancialTestBase
    {
        //success
        [Test]
        public async Task Handle_Query_ShouldReturnListOfReceivedTransaction()
        {
            var projectId = await CreateFinancialProject();

            var createTransaction = new CreateTransactionCommand
            {
                Amount = 100,
                FinancialProjectId = projectId,
                ToUserId = User.Id,
                TransactionType = TransactionType.FinancialReceipts
                
            };

            var id = await SendAsync(createTransaction);
            
            var query = new GetReceivedTransactionQuery();

            var entity = await SendAsync(query);

            entity.Should().NotBeNull();
            entity.Count.Should().Be(1);
            entity.First().Amount.Should().Be(createTransaction.Amount);
            entity.First().Id.Should().Be(id);
            entity.First().ToUser.Id.Should().Be(User.Id);
            entity.First().FromUser.Id.Should().Be(User.Id);
            entity.First().TransactionTypeDto.Name.Should().Be(createTransaction.TransactionType.ToString());
            entity.First().TransactionTypeDto.Value.Should().Be((int)createTransaction.TransactionType);
        }
    }
}