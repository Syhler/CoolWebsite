using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.DeleteFinancialProject;
using FluentAssertions;
using NUnit.Framework;
using CoolWebsite.Domain.Entities.Financial;

namespace Application.IntegrationTests.Financial.FinancialProject.Commands
{
    using static Testing;
    public class DeleteFinancialProjectTests : FinancialTestBase
    {
        
        //TODO(WHAT ABOUT IF PROJECT CONTAINS RECIEPTS)

        [Test]
        public async Task Handle_ValidId_ShouldDelete()
        {
            var id = await CreateFinancialProject();
            
            var command = new DeleteFinancialProjectCommand
            {
                Id = id
            };

            await SendAsync(command);

            var entity = await FindAsync<CoolWebsite.Domain.Entities.Financial.FinancialProject>(id);

            entity.Should().BeNull();

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