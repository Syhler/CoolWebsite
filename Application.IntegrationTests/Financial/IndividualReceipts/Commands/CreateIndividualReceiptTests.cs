using System;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.CreateIndividualReceipt;
using CoolWebsite.Domain.Entities.Financial;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.IndividualReceipts.Commands
{
    using static Testing;
    
    public class CreateIndividualReceiptTests : FinancialTestBase
    {

        [Test]
        public async Task Handle_ValidFields_ShouldCreateIndividualReceipt()
        {
            var receiptId = await CreateReceipt();
            
            var user = await RunAsDefaultUserAsync();

            await AddAsync(user);
            
            var command = new CreateIndividualReceiptCommand
            {
                ReceiptId = receiptId,
                Total = 10000,
                UserId = user.Id
            };

            var id = await SendAsync(command);

            var context = Context();

            var entity = context.IndividualReceipts.Include(x => x.User).FirstOrDefault(y => y.Id == id);

            await context.DisposeAsync();

            entity.Should().NotBeNull();
            entity.Total.Should().Be(command.Total);
            entity.UserId.Should().Be(command.UserId);
            entity.User.UserName.Should().Be(user.UserName);
            entity.ReceiptId.Should().Be(command.ReceiptId);
            entity.Created.Should().BeCloseTo(DateTime.Now, 1000);
            entity.CreatedBy.Should().Be(command.UserId);

        }
        
        [Test]
        public async Task Handle_TotalBelowZero_ShowThrowValidationError()
        {
            var command = new CreateIndividualReceiptCommand
            {
                ReceiptId = "dasd",
                Total = -55,
                UserId = "asdasd"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Handle_ReceiptIdEmpty_ShouldThrowValidationError()
        {
            var command = new CreateIndividualReceiptCommand
            {
                ReceiptId = "",
                Total = -55,
                UserId = "asdasd"
            };
            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test]
        public async Task Handle_UserIdEmpty_ShouldThrowValidationError()
        {
            var command = new CreateIndividualReceiptCommand
            {
                ReceiptId = "dasd",
                Total = -55,
                UserId = ""
            };
            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }
    }
}