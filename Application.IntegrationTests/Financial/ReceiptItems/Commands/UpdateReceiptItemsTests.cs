using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.UpdateReceiptItems;
using CoolWebsite.Domain.Entities.Enums;
using CoolWebsite.Domain.Entities.Financial;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.ReceiptItems.Commands
{
    using static Testing;
    
    public class UpdateReceiptItemsTests : FinancialTestBase
    {
        
        [Test]
        public async Task Handle_ValidId_ShouldUpdateEntity()
        {
            var receiptId = await CreateReceipt();

            
            var createCommand = new CreateReceiptItemCommand
            {
                ReceiptId = receiptId,
                Price = 2,
                ItemGroup = (int)ItemGroup.Essentials,
                Count = 22,
                Name = "das"
            };

            var id = await SendAsync(createCommand);
            
            
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Miscellaneous,
                Name = "not even a name lmao",
                Price = 231.32321,
                ReceiptItemId = id
            };

            await SendAsync(updateCommand);

            var entity = await FindAsync<ReceiptItem>(id);

            entity.Should().NotBeNull();
            entity.Count.Should().Be(updateCommand.Count);
            entity.ItemGroup.Should().Be(updateCommand.ItemGroup);
            entity.Name.Should().Be(updateCommand.Name);
            entity.Price.Should().Be(updateCommand.Price);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(User.Id);

        }
        [Test]
        public void Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Name = "not even a name lmao",
                Price = 231.32321,
                ReceiptItemId = "asdasdas"
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
                Name = "not even a name lmao",
                Price = 231.32321,
                ReceiptItemId = "dont even matter lmao"
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        
        [Test]
        public void Handle_NameEmpty_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                ItemGroup = (int)ItemGroup.Essentials,
                Name = "",
                Price = 231.32321,
                ReceiptItemId = "dont even matter lmao"
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
                Name = "asd",
                Price = -231.32321,
                ReceiptItemId = "dont even matter lmao"
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
        
       
        [Test]
        public void Handle_ItemGroupNull_ShouldThrowValidationException()
        {
            var updateCommand = new UpdateReceiptItemCommand
            {
                Count = 1235,
                Name = "asd",
                Price = 231.32321,
                ReceiptItemId = "dont even matter lmao"
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
                Name = "asd",
                Price = 231.32321
            };

            FluentActions.Awaiting(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
        }
    }
}