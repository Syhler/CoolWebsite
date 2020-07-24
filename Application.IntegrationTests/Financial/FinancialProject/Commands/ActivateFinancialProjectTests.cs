using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.ActivateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.DeleteFinancialProject;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.FinancialProject.Commands
{
    using static Testing;
    
    public class ActivateFinancialProjectTests : FinancialTestBase
    {
    
        //valid
        [Test]
        public async Task Handle_ValidId_ShouldActivateProject()
        {
            var projectId = await CreateFinancialProject();
            
            var delete = new DeleteFinancialProjectCommand{Id = projectId};

            await SendAsync(delete);

            var entity = await FindAsync<CoolWebsite.Domain.Entities.Financial.FinancialProject>(projectId);
            entity.Deleted.Should().BeCloseTo(DateTime.Now, 1000);
            entity.DeletedByUserId.Should().Be(User.Id);
            
            var activateCommand = new ActivateFinancialProjectCommand
            {
                ProjectId = projectId
            };

            await SendAsync(activateCommand);
            
            var activatedEntity = await FindAsync<CoolWebsite.Domain.Entities.Financial.FinancialProject>(projectId);

            activatedEntity.Deleted.Should().Be(null);
            activatedEntity.DeletedByUserId.Should().Be(null);

        }
        
        
        
        [Test]
        public async Task Handle_ValidIdNotCreatedByUser_ShouldThrowNotFoundException()
        {
            var projectId = await CreateFinancialProject();
            
            var delete = new DeleteFinancialProjectCommand{Id = projectId};

            await SendAsync(delete);

            var entity = await FindAsync<CoolWebsite.Domain.Entities.Financial.FinancialProject>(projectId);
            entity.Deleted.Should().BeCloseTo(DateTime.Now, 1000);
            entity.DeletedByUserId.Should().Be(User.Id);
            
            var activateCommand = new ActivateFinancialProjectCommand
            {
                ProjectId = projectId
            };

            await RunAsUserAsync("newUser@d.com", "nah");
            
            
            
            FluentActions.Invoking(async () => await SendAsync(activateCommand)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void Handle_InvalidId_ShouldThrowNotFoundException()
        {
            var activateCommand = new ActivateFinancialProjectCommand
            {
                ProjectId = "nah"
            };
            
            FluentActions.Invoking(async () => await SendAsync(activateCommand)).Should().Throw<NotFoundException>();
        }

        [Test]
        public void Handle_NullId_ShouldThrowValidationException()
        {
            var activateCommand = new ActivateFinancialProjectCommand
            {
                ProjectId = null!
            };

            FluentActions.Invoking(async () => await SendAsync(activateCommand)).Should().Throw<ValidationException>();
        }

        [Test]
        public void Handle_EmptyId_ShouldThrowValidationException()
        {
            var activateCommand = new ActivateFinancialProjectCommand
            {
                ProjectId = ""
            };

            FluentActions.Invoking(async () => await SendAsync(activateCommand)).Should().Throw<ValidationException>();
        }
        
        
    }
}