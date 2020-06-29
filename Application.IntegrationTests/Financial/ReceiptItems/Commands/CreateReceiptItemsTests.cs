using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.ReceiptItems.Commands
{
    using static Testing;
    
    public class CreateReceiptItemsTests : FinancialTestBase
    {
        
        [Test]
        public async Task Handle_ValidFields_CreateEntity()
        {
            var receiptId = await CreateReceipt();

           
            
            var create = new CreateReceiptItemCommand
            {
                Count = 5,
                Price = 2,
                Name = "test",
                ItemGroup = (int)ItemGroup.Essentials,
                ReceiptId = receiptId,
                UsersId = new List<string>
                {
                    SecondUser.Id
                }
            };

            var id = await SendAsync(create);

            var context = Context();
            
            var entity = await context.ReceiptItems
                .Include(x => x.Users)
                .Include(x => x.Receipt)
                .FirstOrDefaultAsync(x => x.Id == id);

            var records = context.OweRecords.Where(x => x.FinancialProjectId == entity.Receipt.FinancialProjectId);
            
            
            
            entity.Should().NotBeNull();
            entity.Count.Should().Be(create.Count);
            entity.Price.Should().Be(create.Price);
            entity.Name.Should().Be(create.Name);
            entity.ItemGroup.Should().Be(create.ItemGroup);
            entity.Users.First().ApplicationUserId.Should().Be(SecondUser.Id);
            entity.Users.First().ReceiptItemId.Should().Be(id);
            entity.ReceiptId.Should().Be(receiptId);
            entity.CreatedBy.Should().Be(User.Id);
            entity.Created.Should().BeCloseTo(DateTime.Now, 1000);

            var firstOrDefault = records.FirstOrDefault(x => x.OwedUserId == User.Id);
            firstOrDefault?.Amount.Should().Be(create.Count * create.Price);
        }

        [Test]
        public void Handle_NameEmpty_ThrowValidationException()
        {
            var create = new CreateReceiptItemCommand
            {
                Count = 5,
                Price = 2,
                Name = "",
                ItemGroup = (int)ItemGroup.Essentials,
                ReceiptId = "asdsa",
                UsersId = new List<string>{User.Id}
            };
            
            FluentActions.Awaiting(() => SendAsync(create)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_PriceBelowZero_ThrowValidationException()
        {
            var create = new CreateReceiptItemCommand
            {
                Count = 5,
                Price = -2,
                Name = "sds",
                ItemGroup = (int)ItemGroup.Essentials,
                ReceiptId = "asdsa",
                UsersId = new List<string>{User.Id}
            };
            
            FluentActions.Awaiting(() => SendAsync(create)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_CountBelowZero_ThrowValidationException()
        {
            var create = new CreateReceiptItemCommand
            {
                Count = -5,
                Price = 2,
                Name = "sds",
                ItemGroup = (int)ItemGroup.Essentials,
                ReceiptId = "asdsa",
                UsersId = new List<string>{User.Id}
            };
            
            FluentActions.Awaiting(() => SendAsync(create)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_ItemGroupNull_ThrowValidationException()
        {
            var create = new CreateReceiptItemCommand
            {
                Count = 2,
                Price = 2,
                Name = "sds",
                ReceiptId = "asdsa",
                UsersId = new List<string>{User.Id}
            };
            
            FluentActions.Awaiting(() => SendAsync(create)).Should().Throw<ValidationException>();
        }
        
        //ReceiptIdEmpty
        [Test]
        public void Handle_ReceiptIdEmpty_ThrowValidationException()
        {
            var create = new CreateReceiptItemCommand
            {
                Count = 2,
                Price = 2,
                Name = "sds",
                ReceiptId = "",
                ItemGroup = (int)ItemGroup.Essentials,
                UsersId = new List<string>{User.Id}
                
            };
            
            FluentActions.Awaiting(() => SendAsync(create)).Should().Throw<ValidationException>();
        }
        
        [Test]
        public void Handle_InvalidId_ShouldThrowParentObjectNotFoundException()
        {
            var create = new CreateReceiptItemCommand
            {
                Count = 2,
                Price = 2,
                Name = "sds",
                ReceiptId = "asdsa",
                ItemGroup = (int)ItemGroup.Essentials,
                UsersId = new List<string>{User.Id}
            };

            FluentActions.Awaiting(() => SendAsync(create)).Should().Throw<ParentObjectNotFoundException>();
        }

        [Test]
        public void Handle_UserIdsNull_ThrowValidationException()
        {
            var create = new CreateReceiptItemCommand
            {
                Count = 2,
                Price = 2,
                Name = "sds",
                ReceiptId = "asdsa",
                ItemGroup = (int)ItemGroup.Essentials
            };

            FluentActions.Awaiting(() => SendAsync(create)).Should().Throw<ValidationException>();
        }
        
        [Test]
        public void Handle_UserIdsEmpty_ThrowValidationException()
        {
            var create = new CreateReceiptItemCommand
            {
                Count = 2,
                Price = 2,
                Name = "sds",
                ReceiptId = "asdsa",
                ItemGroup = (int)ItemGroup.Essentials,
                UsersId = new List<string>()
            };

            FluentActions.Awaiting(() => SendAsync(create)).Should().Throw<ValidationException>();
        }
        
        
        
    }
}