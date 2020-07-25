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
    
    public class GetPayedTransactionTests : FinancialTestBase
    {
        //success
        [Test]
        public async Task Handle_Query_ShouldReturnListOfPayedTransaction()
        {
            var projectId = await CreateFinancialProject();
            
            var createTransaction = new CreateTransactionCommand
            {
                ToUserId = SecondUser.Id,
                TransactionType = TransactionType.FinancialReceipts,
                Amount = 100,
                FinancialProjectId = projectId
            };

            var id = await SendAsync(createTransaction);

            var entities = await SendAsync(new GetPayedTransactionQuery());

            entities.Should().NotBeNull();
            entities.Count.Should().Be(1);
            entities.First().Amount.Should().Be(createTransaction.Amount);
            entities.First().Id.Should().Be(id);
            entities.First().ToUser.Id.Should().Be(SecondUser.Id);
            entities.First().FromUser.Id.Should().Be(User.Id);
            entities.First().TransactionTypeDto.Name.Should().Be(createTransaction.TransactionType.ToString());
            entities.First().TransactionTypeDto.Value.Should().Be((int)createTransaction.TransactionType);
            
        }
    }
}