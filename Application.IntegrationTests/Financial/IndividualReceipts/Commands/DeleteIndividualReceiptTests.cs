using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.IndividualReceipts.Commands.DeleteIndividualReceipt;
using CoolWebsite.Domain.Entities.Financial;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.IndividualReceipts.Commands
{
    using static Testing;
    public class DeleteIndividualReceiptTests : FinancialTestBase
    {
        [Test]
        public async Task Handle_ValidId_ShouldDeleteIndividualReceipt()
        {
            var individualReceipt = await CreateIndividualReceipt();
            
            var deleteCommand = new DeleteIndividualReceiptCommand
            {
                Id = individualReceipt.Id
            };

            var notDeleted = await FindAsync<IndividualReceipt>(individualReceipt.Id);

            notDeleted.Should().NotBeNull();
            
            await SendAsync(deleteCommand);

            var entity = await FindAsync<IndividualReceipt>(deleteCommand.Id);

            entity.Should().BeNull();

        }
        
        [Test]
        public async Task Handle_InvalidId_ShouldThrowNotFoundException()
        {
            
            var deleteCommand = new DeleteIndividualReceiptCommand
            {
                Id = "asdasd"
            };

            FluentActions.Invoking(async () => await SendAsync(deleteCommand)).Should().Throw<NotFoundException>();
        }
        
        [Test]
        public async Task Handle_IdEmpty_ShouldThrowValidationException()
        {
            var deleteCommand = new DeleteIndividualReceiptCommand
            {
                Id = ""
            };

            FluentActions.Invoking(async () => await SendAsync(deleteCommand)).Should().Throw<ValidationException>();
        }
        
    }
}