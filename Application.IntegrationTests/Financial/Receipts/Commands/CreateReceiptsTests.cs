using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Command.CreateReceipts;
using CoolWebsite.Domain.Entities.Financial;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.Reciepts.Commands
{
    using static Testing;
    
    public class CreateReceiptsTests : FinancialTestBase
    {

        //TODO(revist this when done implemting invidualReceipt)
        
        [Test]
        public async Task Handle_ValidFields_ShouldCreateEntity()
        {
            var id = await CreateFinancialProject();
            
            var user = await RunAsDefaultUserAsync();

            var command = new CreateReceiptsCommand
            {
                FinancialProjectId = id,
                Total = 100,
                Receiptors = new List<IndividualReceipt>()
            };

            var receiptsId = await SendAsync(command);

            var context = Context();

            var entity = context.Receipts.Include(x => x.Receptors)
                .FirstOrDefault(y => y.Id == receiptsId);

            entity.Should().NotBeNull();
            entity.Total.Should().Be(command.Total);
            entity.Receptors.Should().BeEmpty(); //TODO(REDO)
            entity.FinancialProjectId.Should().Be(command.FinancialProjectId);
            entity.Created.Should().BeCloseTo(DateTime.Now, 10000);
            entity.CreatedBy.Should().Be(user.Id);

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
            };

            var receiptsId = await SendAsync(command);

            var context = Context();

            var entity = context.Receipts.Include(x => x.Receptors)
                .FirstOrDefault(y => y.Id == receiptsId);

            entity.Should().NotBeNull();
            entity.Total.Should().Be(command.Total);
            entity.Receptors.Should().BeEmpty();
            entity.FinancialProjectId.Should().Be(command.FinancialProjectId);
            entity.Created.Should().BeCloseTo(DateTime.Now, 10000);
            entity.CreatedBy.Should().Be(user.Id);
        }
        
        [Test]
        public async Task Handle_TotalBelowZero_ShouldThrowValidationException()
        {
            var id = await CreateFinancialProject();
            
            var command = new CreateReceiptsCommand
            {
                FinancialProjectId = id,
                Total = -5,
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_FinancialProjectIdEmpty_ShouldThrowValidationException()
        {
            var command = new CreateReceiptsCommand
            {
                Total = 100,
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}