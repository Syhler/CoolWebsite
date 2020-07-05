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
            
            var command = new CreateReceiptsCommand
            {
                FinancialProjectId = id,
                Total = 100,
                BoughtAt = DateTime.Now,
                Title = "Title"
            };

            var receiptsId = await SendAsync(command);

            var entity = await FindAsync<Receipt>(receiptsId);

            entity.Should().NotBeNull();
            entity.FinancialProjectId.Should().Be(command.FinancialProjectId);
        
            entity.Created.Should().BeCloseTo(DateTime.Now, 10000);
            entity.CreatedBy.Should().Be(User.Id);
            entity.Location.Should().Be(command.Title);
            entity.DateVisited.Should().BeCloseTo(DateTime.Now, 1000);

        }

        [Test]
        public async Task Handle_NoUsers_ShouldCreateEntity()
        {
            var id = await CreateFinancialProject();
            
            var user = await RunAsDefaultUserAsync();

            var command = new CreateReceiptsCommand
            {
                FinancialProjectId = id,
                Total = 100,
                BoughtAt = DateTime.Now,
                Title = "Title"
            };

            var receiptsId = await SendAsync(command);

            var entity = await FindAsync<Receipt>(receiptsId);

            entity.Should().NotBeNull();
            entity.FinancialProjectId.Should().Be(command.FinancialProjectId);
            entity.Created.Should().BeCloseTo(DateTime.Now, 1000);
            entity.CreatedBy.Should().Be(user.Id);
            entity.DateVisited.Should().BeCloseTo(DateTime.Now, 1000);
            entity.Location.Should().Be(command.Title);
        }
        
        [Test]
        public async Task Handle_TotalBelowZero_ShouldThrowValidationException()
        {
            var id = await CreateFinancialProject();
            
            var command = new CreateReceiptsCommand
            {
                FinancialProjectId = id,
                Total = -5,
                BoughtAt = DateTime.Now,
                Title = "Title"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_FinancialProjectIdEmpty_ShouldThrowValidationException()
        {
            var command = new CreateReceiptsCommand
            {
                Total = 100,
                BoughtAt = DateTime.Now,
                Title = "Title",
                FinancialProjectId = ""
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_BoughtAtEmpty_ShouldThrowValidationException()
        {
            var command = new CreateReceiptsCommand
            {
                Total = 100,
                Title = "Title",
                FinancialProjectId = "sad"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_TitleEmpty_ShouldThrowValidationException()
        {
            var command = new CreateReceiptsCommand
            {
                Total = 100,
                BoughtAt = DateTime.Now,
                Title = "",
                FinancialProjectId = "asd"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_TitleAboveMaxLength_ShouldThrowValidationException()
        {
            var command = new CreateReceiptsCommand
            {
                Total = 100,
                BoughtAt = DateTime.Now,
                Title = "dsfsfsdfijpsdkjfpsdkfpodskofkdsokposfkpospkofsdkpofsdkpofsdkposdfpkokfpsdkfspdopk" +
                        "ogfddgfopkfgdkppkdgfopkogdfkpogdfkpodgfkpogdfkpogdfkpogdfkpofgdpkokpgfdopk",
                FinancialProjectId = "asd"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
        
    }
}