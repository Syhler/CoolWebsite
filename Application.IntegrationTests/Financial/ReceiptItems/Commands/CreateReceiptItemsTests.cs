using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Commands.CreateReceiptItems;
using CoolWebsite.Domain.Entities.Enums;
using CoolWebsite.Domain.Entities.Financial;
using FluentAssertions;
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
                ItemGroup = ItemGroup.Meat,
                ReceiptId = receiptId
            };

            var id = await SendAsync(create);

            var entity = await FindAsync<ReceiptItem>(id);

            entity.Should().NotBeNull();
            entity.Count.Should().Be(create.Count);
            entity.Price.Should().Be(create.Price);
            entity.Name.Should().Be(create.Name);
            entity.ItemGroup.Should().Be(create.ItemGroup);
            entity.ReceiptId.Should().Be(receiptId);
        }

        [Test]
        public void Handle_NameEmpty_ThrowValidationException()
        {
            var create = new CreateReceiptItemCommand
            {
                Count = 5,
                Price = 2,
                Name = "",
                ItemGroup = ItemGroup.Meat,
                ReceiptId = "asdsa"
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
                ItemGroup = ItemGroup.Meat,
                ReceiptId = "asdsa"
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
                ItemGroup = ItemGroup.Meat,
                ReceiptId = "asdsa"
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
                ReceiptId = "asdsa"
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
                ItemGroup = ItemGroup.Meat
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
                ItemGroup = ItemGroup.Meat
            };

            FluentActions.Awaiting(() => SendAsync(create)).Should().Throw<ParentObjectNotFoundException>();
        }
        
    }
}