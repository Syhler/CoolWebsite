using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.CreateTransaction;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Transaction.Queries
{
    using static Testing;
    
    public class GetPayedTransactionByProjectTests : FinancialTestBase
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

            var query = new GetPayedTransactionByProjectQuery
            {
                FinancialProjectId = projectId
            };
            
            var entities = await SendAsync(query);

            entities.Should().NotBeNull();
            entities.Count.Should().Be(1);
            entities.First().Amount.Should().Be(createTransaction.Amount);
            entities.First().Id.Should().Be(id);
            entities.First().ToUser.Id.Should().Be(SecondUser.Id);
            entities.First().FromUser.Id.Should().Be(User.Id);
            entities.First().TransactionTypeDto.Name.Should().Be(createTransaction.TransactionType.ToString());
            entities.First().TransactionTypeDto.Value.Should().Be((int)createTransaction.TransactionType);
        }
        
        //wrongId
        [Test]
        public async Task Handle_InvalidId_ShouldReturnEmptyList()
        {
            var query = new GetPayedTransactionByProjectQuery
            {
                FinancialProjectId = "Invalid"
            };
            
            var entities = await SendAsync(query);

            entities.Should().NotBeNull();
            entities.Count.Should().Be(0);
        }
        
        //idempty
        [Test]
        public void Handle_IdIsEmpty_ShouldThrowValidationException()
        {
            var query = new GetPayedTransactionByProjectQuery
            {
                FinancialProjectId = ""
            };

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<ValidationException>();
        }
        
        //id null
        [Test]
        public void Handle_IdIsNull_ShouldThrowValidationException()
        {
            var query = new GetPayedTransactionByProjectQuery
            {
                FinancialProjectId = null!
            };

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<ValidationException>();
        }
    }
}