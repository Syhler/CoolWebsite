using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.UpdateReceipts;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.Receipts.Commands
{
    using static Testing;
    
    public class UpdateReceiptsTests : FinancialTestBase
    {

        [Test]
        public async Task Handle_ValidFields_ShouldUpdate()
        {
            var projectId = await CreateFinancialProject();
            var id = await CreateReceipt(projectId);

            var itemCommand = new CreateReceiptItemCommand
            {
                Name = "Receipt",
                Count = 10,
                Price = 1000,
                ItemGroup = 0,
                ReceiptId = id,
                UsersId = new List<string>
                {
                    SecondUser.Id
                }
            };

            var receiptItemId = await SendAsync(itemCommand);
            
            var item = GetReceiptItem(5);
            
            var context = CreateContext();

            var oweRecord = context.OweRecords.FirstOrDefault(x => x.OwedUserId == User.Id && x.UserId == SecondUser.Id && x.FinancialProjectId == projectId);

            oweRecord.Should().NotBeNull();
            oweRecord.Amount.Should().Be(itemCommand.Count * itemCommand.Price);

            var editedItem = new ReceiptItemDto
            {
                ItemGroup = new ItemGroupDto {Value = itemCommand.ItemGroup},
                Count = 1,
                Id = receiptItemId,
                Price = 100,
                Users = new List<UserDto> {new UserDto {Id = SecondUser.Id}}
            };
            
            var command = new UpdateReceiptCommand
            {
                Id = id,
                Location = "Netto",
                Datevisited = DateTime.Now,
                Note = "meh",
                ItemDtos = new List<ReceiptItemDto>
                {
                    item,
                   editedItem
                }
            };

            await SendAsync(command);


            var entity = context.Receipts
                .Include(x => x.Items)
                .FirstOrDefault(x => x.Id == command.Id);

            context = CreateContext();
            
            var newOweRecord = context.OweRecords.FirstOrDefault(x => x.OwedUserId == User.Id && x.UserId == SecondUser.Id && x.FinancialProjectId == projectId);

            newOweRecord.Should().NotBeNull();
            newOweRecord.Amount.Should().Be((item.Price*item.Count/item.Users.Count) + editedItem.Price*editedItem.Count/editedItem.Users.Count);
            
            entity.Should().NotBeNull();
            entity.Note.Should().Be(command.Note);
            entity.Items.FirstOrDefault(x => x.Id == item.Id).Should().NotBeNull();
            entity.Items.Count.Should().Be(2);
            
            
            var editedItemEntity = entity.Items.FirstOrDefault(x => x.Id == editedItem.Id);
            editedItemEntity.Should().NotBeNull();
            editedItemEntity.Count.Should().Be(editedItem.Count);
            editedItemEntity.Price.Should().Be(editedItem.Price);
            editedItemEntity.ItemGroup.Should().Be(editedItem.ItemGroup.Value);
            
            var newItem = entity.Items.FirstOrDefault(x => x.Id == item.Id);
            newItem.Should().NotBeNull();
            newItem.Count.Should().Be(item.Count);
            newItem.Price.Should().Be(item.Price);
            newItem.ItemGroup.Should().Be(item.ItemGroup.Value);
            
            
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(User.Id);
            entity.DateVisited.Should().BeCloseTo(DateTime.Now, 1000);
            entity.Location.Should().Be(command.Location);
        }

        [Test]
        public async Task Handle_WithoutFinancialProjectId_ShouldUpdate()
        {
            var projectId = await CreateFinancialProject();
            var id = await CreateReceipt(projectId);
            var user = await RunAsDefaultUserAsync();
            
            var command = new UpdateReceiptCommand
            {
                Id = id,
                Location = "Title",
                Datevisited = DateTime.Now
            };

            await SendAsync(command);

            var entity = await FindAsync<Receipt>(command.Id);

            entity.Should().NotBeNull();
            entity.FinancialProjectId.Should().Be(projectId);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(user.Id);
            entity.DateVisited.Should().BeCloseTo(DateTime.Now, 1000);
            entity.Location.Should().Be(command.Location);
        }

        [Test]
        public async Task Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "nah",
                Location = "Title",
                Datevisited = DateTime.Now
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task Handle_IdEmpty_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "",
                Location = "Title",
                Datevisited = DateTime.Now
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        
        [Test]
        public async Task Handle_BoughtAtEmpty_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "asdadas",
                Location = "Title",
            };
            
            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test]
        public async Task Handle_TitleEmpty_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "asdadas",
                Datevisited = DateTime.Now,
                Location = ""
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_TitleAboveMaxLength_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptCommand
            {
                Id = "asdadas",
                Location = "dafhdsugjhsfdosjdfjiodsfoijdsfjiosdfsdfdfsdfsdfsdfsfsdfsdfsdf" +
                        "dfgdfgdfgojifdsjoifsdjoisdfojisdfjoisdfjoifsojdijoisdfjoifsdoij",
                Datevisited = DateTime.Now
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
        
    }
}