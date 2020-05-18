using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects;
using CoolWebsite.Domain.Entities.Identity;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.FinancialProject.Queries
{
    using static Testing;
    
    public class GetFinancialProjectByIdQueryTests : FinancialTestBase
    {
        
        
        [Test]
        public async Task Handle_ValidProjectId_ShouldReturnProject()
        {
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = "test",
                Users = new List<ApplicationUser>
                {
                    User
                }
            };

            var project = await SendAsync(createCommand);

            var query = new GetFinancialProjectByIdQuery
            {
                ProjectId = project.Id
            };

            var model = await SendAsync(query);

            model.Should().NotBeNull();
            model.Id.Should().Be(project.Id);
            model.Created.Should().BeCloseTo(DateTime.Now, 1000);
            model.Users.First().Id.Should().Be(User.Id);
            model.Title.Should().Be(project.Title);
        }

        [Test]
        public async Task Handle_InvalidProjectId_ShouldThrowNotFoundException()
        {
         
            var query = new GetFinancialProjectByIdQuery
            {
                ProjectId = "asdasdas"
            };

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<NotFoundException>();

        }

        [Test]
        public async Task Handle_EmptyProjectId_ShouldThrowValidationException()
        {
            var query = new GetFinancialProjectByIdQuery
            {
                ProjectId = ""
            };

            FluentActions.Invoking(async () => await SendAsync(query)).Should().Throw<ValidationException>();
        }

    }
}