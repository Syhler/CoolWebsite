using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.UpdateIndividualReceipt;
using CoolWebsite.Domain.Entities.Financial;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.IndividualReceipts.Commands
{
    using static Testing;
    
    public class UpdateIndividualReceiptTests : FinancialTestBase
    {
        
        
        [Test]
        public async Task Handle_ValidFields_ShouldUpdateIndividualReceipt()
        {
            var individualReceipt = await CreateIndividualReceipt();
            var receiptId = await CreateReceipt();
            var user = await RunAsDefaultUserAsync();

            await AddAsync(user);

            var command = new UpdateIndividualReceiptCommand
            {
                Id = individualReceipt.Id,
                UserId = user.Id,
                ReceiptId = receiptId,
                Total = 100003123
            };

            await SendAsync(command);

            var entity = await FindAsync<IndividualReceipt>(command.Id);

            entity.Should().NotBeNull();
            entity.UserId.Should().Be(command.UserId);
            entity.ReceiptId.Should().Be(command.ReceiptId);
            entity.Total.Should().Be(command.Total);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(user.Id);

        }
        
        [Test]
        public async Task Handle_ValidFieldsWithoutUserID_ShouldUpdateIndividualReceipt()
        {
            var individualReceipt = await CreateIndividualReceipt();
            var user = await RunAsDefaultUserAsync();

            await AddAsync(user);

            var command = new UpdateIndividualReceiptCommand
            {
                Id = individualReceipt.Id,
                UserId = user.Id,
                Total = 100003123
            };

            await SendAsync(command);

            var entity = await FindAsync<IndividualReceipt>(command.Id);

            entity.Should().NotBeNull();
            entity.UserId.Should().Be(command.UserId);
            entity.ReceiptId.Should().Be(individualReceipt.ReceiptId);
            entity.Total.Should().Be(command.Total);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(user.Id);
            
        }
        
        [Test]
        public async Task Handle_ValidFields_WithoutReceiptId_ShouldUpdateIndividualReceipt()
        {
            var individualReceipt = await CreateIndividualReceipt();
            var receiptId = await CreateReceipt();
            var user = await RunAsDefaultUserAsync();

            await AddAsync(user);

            var command = new UpdateIndividualReceiptCommand
            {
                Id = individualReceipt.Id,
                ReceiptId = receiptId,
                Total = 100003123
            };

            await SendAsync(command);

            var entity = await FindAsync<IndividualReceipt>(command.Id);

            entity.Should().NotBeNull();
            entity.UserId.Should().Be(individualReceipt.UserId);
            entity.ReceiptId.Should().Be(command.ReceiptId);
            entity.Total.Should().Be(command.Total);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(user.Id);
            
        }
        
        [Test]
        public async Task Handle_TotalBelowZero_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateIndividualReceiptCommand
            {
                Id = "asd",
                Total = -5
            };
            
            FluentActions.Invoking(async () => await SendAsync(updateCommand)).Should().Throw<ValidationException>();

        }
        
        [Test]
        public async Task Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var updateCommand = new UpdateIndividualReceiptCommand
            {
                Id = "asdasd",
                Total = 123
            };

            FluentActions.Invoking(async () => await SendAsync(updateCommand)).Should().Throw<NotFoundException>();
        }
        
       
        
        
    }
}