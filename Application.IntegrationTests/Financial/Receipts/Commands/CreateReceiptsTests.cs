using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.Receipts.Commands.CreateReceipts;
using CoolWebsite.Domain.Entities.Financial;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.Receipts.Commands
{
    using static Testing;
    
    public class CreateReceiptsTests : FinancialTestBase
    {

        [Test]
        public async Task Handle_ValidFields_ShouldCreateEntity()
        {
            var id = await CreateFinancialProject();
            
            var command = new CreateReceiptCommand
            {
                FinancialProjectId = id,
                DateVisited = DateTime.Now,
                Location = "Title"
            };

            var receiptsId = await SendAsync(command);

            var entity = await FindAsync<Receipt>(receiptsId);

            entity.Should().NotBeNull();
            entity.FinancialProjectId.Should().Be(command.FinancialProjectId);
        
            entity.Created.Should().BeCloseTo(DateTime.Now, 10000);
            entity.CreatedBy.Should().Be(User.Id);
            entity.Location.Should().Be(command.Location);
            entity.DateVisited.Should().BeCloseTo(DateTime.Now, 1000);

        }

        [Test]
        public void Handle_FinancialProjectIdEmpty_ShouldThrowValidationException()
        {
            var command = new CreateReceiptCommand
            {
                DateVisited = DateTime.Now,
                Location = "Title",
                FinancialProjectId = ""
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_BoughtAtEmpty_ShouldThrowValidationException()
        {
            var command = new CreateReceiptCommand
            {
                Location = "Title",
                FinancialProjectId = "sad"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_TitleEmpty_ShouldThrowValidationException()
        {
            var command = new CreateReceiptCommand
            {
                DateVisited = DateTime.Now,
                Location = "",
                FinancialProjectId = "asd"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_TitleAboveMaxLength_ShouldThrowValidationException()
        {
            var command = new CreateReceiptCommand
            {
                DateVisited = DateTime.Now,
                Location = "dsfsfsdfijpsdkjfpsdkfpodskofkdsokposfkpospkofsdkpofsdkpofsdkposdfpkokfpsdkfspdopk" +
                        "ogfddgfopkfgdkppkdgfopkogdfkpogdfkpodgfkpogdfkpogdfkpogdfkpofgdpkokpgfdopk",
                FinancialProjectId = "asd"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
        
    }
}