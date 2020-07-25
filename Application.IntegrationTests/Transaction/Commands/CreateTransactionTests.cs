using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Commands.CreateTransaction;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Transaction.Commands
{
    using static Testing;
    
    
    public class CreateTransactionTests : FinancialTestBase
    {
        //Works
        [Test]
        public async Task Handle_ValidFields_ShouldCreateTransaction()
        {
            var project = await CreateFinancialProject();

            var command = new CreateTransactionCommand
            {
                TransactionType = TransactionType.FinancialReceipts,
                FinancialProjectId = project,
                ToUserId = SecondUser.Id,
                Amount = 1000
            };

            var id = await SendAsync(command);

            var entity = await FindAsync<CoolWebsite.Domain.Entities.Financial.Transaction>(id);

            entity.Should().NotBeNull();
            entity.Amount.Should().Be(command.Amount);
            entity.TransactionType.Should().Be(command.TransactionType);
            entity.FinancialProjectId.Should().Be(command.FinancialProjectId);
            entity.ToUserId.Should().Be(command.ToUserId);
            entity.CreatedBy.Should().Be(User.Id);
            entity.Created.Should().BeCloseTo(DateTime.Now, 1000);
        }
        
        //greateer than 0
        [Test]
        public void Handle_AmountBelowZero_ShouldThrowValidationException()
        {
            var command = new CreateTransactionCommand
            {
                TransactionType = TransactionType.FinancialReceipts,
                FinancialProjectId = "DASDASD",
                ToUserId = SecondUser.Id,
                Amount = -10
            };

            FluentActions.Invoking(async () => { await SendAsync(command);}).Should().Throw<ValidationException>();
        }
        

        //notnull
        [Test]
        public void Handle_FinancialProjectIdIsNull_ShouldThrowValidationException()
        {
            var command = new CreateTransactionCommand
            {
                TransactionType = TransactionType.FinancialReceipts,
                FinancialProjectId = null,
                ToUserId = SecondUser.Id,
                Amount = 1000
            };

            FluentActions.Invoking(async () => { await SendAsync(command);}).Should().Throw<ValidationException>();
        }
        
        //notempty
        [Test]
        public void Handle_FinancialProjectIsEmpty_ShouldThrowValidationException()
        {
            var command = new CreateTransactionCommand
            {
                TransactionType = 0,
                FinancialProjectId = "",
                ToUserId = SecondUser.Id,
                Amount = 1000
            };

            FluentActions.Invoking(async () => { await SendAsync(command);}).Should().Throw<ValidationException>();
        }
        
        //notnull
        [Test]
        public void Handle_ToUserIdIsNull_ShouldThrowValidationException()
        {
            var command = new CreateTransactionCommand
            {
                TransactionType = 0,
                FinancialProjectId = "DASDASD",
                ToUserId = null,
                Amount = 1000
            };

            FluentActions.Invoking(async () => { await SendAsync(command);}).Should().Throw<ValidationException>();
        }
        
        //notempty
        [Test]
        public void Handle_ToUserIdIsEmpty_ShouldThrowValidationException()
        {
            var command = new CreateTransactionCommand
            {
                TransactionType = 0,
                FinancialProjectId = "DASDASD",
                ToUserId = "",
                Amount = 1000
            };

            FluentActions.Invoking(async () => { await SendAsync(command);}).Should().Throw<ValidationException>();
        }
    }
}