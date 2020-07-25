using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.CreateTransaction;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.DeleteTransactions;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Transaction.Commands
{
    using static Testing;
    
    public class DeleteTransactionTests : FinancialTestBase
    {
        //succeed
        [Test]
        public async Task Handle_ValidId_ShouldDeleteTransaction()
        {
            var projectId = await CreateFinancialProject();
            var transaction = new CreateTransactionCommand
            {
                FinancialProjectId = projectId,
                Amount = 100,
                ToUserId = SecondUser.Id,
                TransactionType = TransactionType.FinancialReceipts
            };

            var transactionId = await SendAsync(transaction);

            var context = CreateContext();

            var oweRecord = context.OweRecords.FirstOrDefault(x => x.UserId == User.Id &&
                                                                   x.OwedUserId == SecondUser.Id &&
                                                                   x.FinancialProjectId == projectId);
            oweRecord.Should().NotBeNull();
            oweRecord!.Amount.Should().Be(transaction.Amount * -1);
            
            var deleteTransaction = new DeleteTransactionCommand
            {
                Id = transactionId
            };

            await SendAsync(deleteTransaction);

            context = CreateContext();
            
            oweRecord = context.OweRecords.FirstOrDefault(x => x.UserId == User.Id &&
                                                               x.OwedUserId == SecondUser.Id &&
                                                               x.FinancialProjectId == projectId);
            oweRecord.Should().NotBeNull();
            oweRecord!.Amount.Should().Be(0);

            var entity = await FindAsync<CoolWebsite.Domain.Entities.Financial.Transaction>(transactionId);
            entity.Should().BeNull();

        }
        
        //notfound
        [Test]
        public void Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var deleteTransaction = new DeleteTransactionCommand
            {
                Id = "nope"
            };

            FluentActions.Invoking(async () => await SendAsync(deleteTransaction)).Should().Throw<NotFoundException>();
        }
        
        //wrongUser
        [Test]
        public async Task Handle_WrongUser_ShouldThrowNotFoundException()
        {
            var projectId = await CreateFinancialProject();
            
            var transaction = new CreateTransactionCommand
            {
                FinancialProjectId = projectId,
                Amount = 100,
                ToUserId = SecondUser.Id,
                TransactionType = TransactionType.FinancialReceipts
            };

            var id = await SendAsync(transaction);
            
            var deleteTransaction = new DeleteTransactionCommand
            {
                Id = id
            };

            await RunAsUserAsync("newuser@d.com", "nah");

            FluentActions.Invoking(async () => await SendAsync(deleteTransaction)).Should().Throw<NotFoundException>();

        }
        
        //id empty
        [Test]
        public void Handle_IdIsEmpty_ShouldThrowValidationException()
        {
            var deleteTransaction = new DeleteTransactionCommand
            {
                Id = ""
            };

            FluentActions.Invoking(async () => await SendAsync(deleteTransaction)).Should().Throw<ValidationException>();
        }
        
        //id null
        [Test]
        public void Handle_IdIsNull_ShouldThrowValidationException()
        {
            var deleteTransaction = new DeleteTransactionCommand
            {
                Id = null!
            };

            FluentActions.Invoking(async () => await SendAsync(deleteTransaction)).Should().Throw<ValidationException>();
        }
    }
}