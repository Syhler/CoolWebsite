using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.DeleteFinancialProject;
using FluentAssertions;
using NUnit.Framework;
using CoolWebsite.Domain.Entities.Financial;

namespace Application.IntegrationTests.Financial.FinancialProject.Commands
{
    using static Testing;
    public class DeleteFinancialProjectTests : FinancialTestBase
    {
        
        [Test]
        public async Task Handle_ValidId_ShouldDelete()
        {
            var id = await CreateFinancialProject();

            var receiptId = await CreateReceipt(id);

            var individualReceiptId = await CreateIndividualReceipt(receiptId);
            
            var command = new DeleteFinancialProjectCommand
            {
                Id = id
            };

            var notDeleted = await FindAsync<CoolWebsite.Domain.Entities.Financial.FinancialProject>(id);
            var notDeletedReceipt = await FindAsync<Receipt>(receiptId);
            var notDeletedIndividualReceipt = await FindAsync<IndividualReceiptObsolete>(individualReceiptId);

            notDeleted.Should().NotBeNull();
            notDeletedReceipt.Should().NotBeNull();
            notDeletedIndividualReceipt.Should().NotBeNull();

            await SendAsync(command);

            var entity = await FindAsync<CoolWebsite.Domain.Entities.Financial.FinancialProject>(id);
            var receiptEntity = await FindAsync<Receipt>(receiptId);
            var individualReceiptEntity = await FindAsync<IndividualReceiptObsolete>(individualReceiptId);

            entity.Should().BeNull();
            receiptEntity.Should().BeNull();
            individualReceiptEntity.Should().BeNull();
        }
        
        [Test]
        public async Task Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var command = new DeleteFinancialProjectCommand
            {
                Id = "asdasd"
            };

            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<NotFoundException>();
        }

        public async Task Handle_EmptyId_ShouldThrowValidationException()
        {
            var command = new DeleteFinancialProjectCommand();
            FluentActions.Invoking(async () => await SendAsync(command)).Should().Throw<ValidationException>();
        }

        
        
    }
}