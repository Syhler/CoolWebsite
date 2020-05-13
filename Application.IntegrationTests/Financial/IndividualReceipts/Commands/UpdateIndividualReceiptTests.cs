using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.CreateIndividualReceipt;
using CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.UpdateIndividualReceipt;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;
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
            var user = await RunAsUserAsync("imnew@new","lalal123@123");
            
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
        public async Task Handle_ValidFieldsWithoutReceiptId_ShouldUpdateIndividualReceipt()
        {
            var user = await RunAsUserAsync("newuserlol","passwrod1231");
            
            var createFinancialProjectCommandCommand = new CreateFinancialProjectCommand
            {
                Title = "Create",
                Users = new List<ApplicationUser>
                {
                    user
                }
            };

            var projectId = await SendAsync(createFinancialProjectCommandCommand);
            
            var createReceiptsCommand = new CreateReceiptsCommand
            {
                FinancialProjectId = projectId,
                Total = 0,
                Title = "Title",
                BoughtAt = DateTime.Now
            };

            var receiptId = await SendAsync(createReceiptsCommand);

            var createCommand = new CreateIndividualReceiptCommand
            {
                Total = 0,
                ReceiptId = receiptId,
                UserId = user.Id
            };

            var id = await SendAsync(createCommand);
            

            var command = new UpdateIndividualReceiptCommand
            {
                Id = id,
                UserId = user.Id,
                Total = 100003123
            };

            await SendAsync(command);

            var entity = await FindAsync<IndividualReceipt>(command.Id);

            entity.Should().NotBeNull();
            entity.UserId.Should().Be(command.UserId);
            entity.ReceiptId.Should().Be(receiptId);
            entity.Total.Should().Be(command.Total);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(user.Id);
            
        }
        
        [Test]
        public async Task Handle_ValidFields_WithoutUserId_ShouldUpdateIndividualReceipt()
        {
            var individualReceipt = await CreateIndividualReceipt();
            var receiptId = await CreateReceipt();
            var user = await RunAsDefaultUserAsync();

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