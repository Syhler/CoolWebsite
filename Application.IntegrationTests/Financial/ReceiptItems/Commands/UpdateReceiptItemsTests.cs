using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.UpdateReceiptItems;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.ReceiptItems.Commands
{
    using static Testing;
    
    
    public class UpdateReceiptItemsTests : FinancialTestBase
    {
        
        [Test]
        [TestCase(100, 200)]
        [TestCase(200, 100)]
        public async Task Handle_ValidId_ShouldUpdateEntity(int price, int updatedPrice)
        {

            var newUser = await CreateNewUser("kekw", "kekw");

            var project = await CreateFinancialProject(newUser);
            var receiptId = await CreateReceipt(project);

            
            var createCommand = new CreateReceiptItemCommand
            {
                ReceiptId = receiptId,
                Price = price,
                ItemGroup = (int)ItemGroup.Essentials,
                Count = 2,
                Name = "das",
                UserIds = new List<string>
                {
                    User.Id,
                    newUser.Id
                }
            };

            var itemId = await SendAsync(createCommand);

            
            var updateCommand = new UpdateReceiptItemCommand
            {
                ItemGroup = (int)ItemGroup.Miscellaneous,
                Price = updatedPrice,
                Count = 2,
                Id = itemId,
                UserDtos = new List<UserDto>
                {
                    new UserDto{Id = User.Id},
                    new UserDto{Id = SecondUser.Id}
                },
                FinancialProjectId = project
            };

            await SendAsync(updateCommand);

            var context = CreateContext();

            var entity = await context.ReceiptItems
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == itemId);
                
            entity.Should().NotBeNull();
            entity.Count.Should().Be(updateCommand.Count);
            entity.ItemGroup.Should().Be(updateCommand.ItemGroup);
            entity.Price.Should().Be(updateCommand.Price);
            entity.Users.Count.Should().Be(2);
            entity.Users.Any(x => x.ApplicationUserId == User.Id).Should().Be(true);
            entity.Users.Any(x => x.ApplicationUserId == SecondUser.Id).Should().Be(true);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(User.Id);
            
            //owerecord
            var newOweRecord = context.OweRecords
                .FirstOrDefault(x => x.UserId == SecondUser.Id && x.OwedUserId == User.Id);
            newOweRecord.Amount.Should().Be(
                updateCommand.Price * updateCommand.Count / updateCommand.UserDtos.Count);

            
            var oldOweRecord = context.OweRecords
                .FirstOrDefault(x => x.UserId == newUser.Id && x.OwedUserId == User.Id);
            oldOweRecord.Amount.Should().Be(0);
            

        }
        [Test]
        public void Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Price = 231.32321,
                Id = "asdasdas",
                UserDtos = new List<UserDto>
                {
                    new UserDto{Id = User.Id}
                },
                FinancialProjectId = "asd"
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<NotFoundException>();
        }
        
        
        [Test]
        public void Handle_CountLessThanZero_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = -1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Price = 231.32321,
                Id = "dont even matter lmao",
                UserDtos = new List<UserDto>
                {
                    new UserDto{Id = User.Id}
                },
                FinancialProjectId = "asd"
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        
        
       
        [Test]
        public void Handle_PriceLessThanZero_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Price = -231.32321,
                Id = "dont even matter lmao",
                UserDtos = new List<UserDto>
                {
                    new UserDto{Id = User.Id}
                },
                FinancialProjectId = "asd"
                
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        
       
        [Test]
        public void Handle_ItemGroupNull_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                Price = 231.32321,
                Id = "dont even matter lmao",
                UserDtos = new List<UserDto>
                {
                    new UserDto{Id = User.Id}
                },
                FinancialProjectId = "asd"
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        
        //receiptIdempty
        [Test]
        public void Handle_ReceiptItemIdEmpty_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Price = 231.32321,
                UserDtos = new List<UserDto>
                {
                    new UserDto{Id = User.Id}
                },
                FinancialProjectId = "asd"
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        
        //emptyUsers
        [Test]
        public void Handle_UsersIsEmpty_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Price = 231.32321,
                Id = "asdasdasd",
                UserDtos = new List<UserDto>
                {
                },
                FinancialProjectId = "asd"
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        //nullUsers
        [Test]
        public void Handle_UsersIsNull_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Price = 231.32321,
                Id = "asdasdasd",
                UserDtos = null!,
                FinancialProjectId = "asd"
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        
        
        //Null FinancialId
        [Test]
        public void Handle_FinancialProjectIdIsNull_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Price = 231.32321,
                Id = "asdasdasd",
                UserDtos = new List<UserDto>
                {
                    new UserDto{Id = User.Id}
                },
                FinancialProjectId = null!
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        
        
        //Empty FinancialId
        [Test]
        public void Handle_FinancialProjectIdIsEmpty_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Price = 231.32321,
                Id = "asdasdasd",
                UserDtos = new List<UserDto>
                {
                    new UserDto{Id = User.Id}
                },
                FinancialProjectId = ""
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        
        
    }
}