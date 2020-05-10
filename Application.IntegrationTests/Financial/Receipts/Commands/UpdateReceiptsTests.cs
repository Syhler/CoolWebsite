using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.UpdateReceipts;
using CoolWebsite.Domain.Entities.Financial;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.Reciepts.Commands
{
    using static Testing;
    
    public class UpdateReceiptsTests : FinancialTestBase
    {

        [Test]
        public async Task Handle_ValidFields_ShouldUpdate()
        {
            var projectId = await CreateFinancialProject();
            var id = await CreateReceipt();
            var user = await RunAsDefaultUserAsync();
            
            var command = new UpdateReceiptsCommand
            {
                Id = id,
                Total = 134534.4324,
                FinancialProjectId = projectId
            };

            await SendAsync(command);

            var entity = await FindAsync<Receipt>(command.Id);

            entity.Should().NotBeNull();
            entity.Total.Should().Be(command.Total);
            entity.FinancialProjectId.Should().Be(command.FinancialProjectId);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(user.Id);
        }

        [Test]
        public async Task Handle_WithoutFinancialProjectId_ShouldUpdate()
        {
            var projectId = await CreateFinancialProject();
            var id = await CreateReceipt(projectId);
            var user = await RunAsDefaultUserAsync();
            
            var command = new UpdateReceiptsCommand
            {
                Id = id,
                Total = 134534.4324,
            };

            await SendAsync(command);

            var entity = await FindAsync<Receipt>(command.Id);

            entity.Should().NotBeNull();
            entity.Total.Should().Be(command.Total);
            entity.FinancialProjectId.Should().Be(projectId);
            entity.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            entity.LastModifiedBy.Should().Be(user.Id);
        }

        [Test]
        public async Task Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var command = new UpdateReceiptsCommand
            {
                Total = 2341.3,
                Id = "nah"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task Handle_IdEmpty_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptsCommand
            {
                Total = 2341.3,
                Id = ""
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }


        [Test]
        public async Task Handle_TotalBelowZero_ShouldThrowValidationException()
        {
            var command = new UpdateReceiptsCommand
            {
                Total = -5,
                Id = "asdadas"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
        
    }
}