﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Commands.CreateFinancialProject;
using CoolWebsite.Application.DatabaseAccess.Financial.FinancialProject.Queries.GetFinancialProjects;
using CoolWebsite.Application.DatabaseAccess.Financial.Receipts.Commands.CreateReceipts;
using CoolWebsite.Domain.Entities.Identity;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.FinancialProject.Queries
{
    using static Testing;
    
    public class GetAllFinancialProjectsTests : FinancialTestBase
    {
        [Test]
        public async Task Handle_ValidUserId_ShouldReturnProject()
        {
            var user = await RunAsDefaultUserAsync();
            await AddAsync(user);
            
            var createCommand = new CreateFinancialProjectCommand
            {
                Title = "Create",
                Users = new List<ApplicationUser>
                {
                    user
                }
            };

            var id = await SendAsync(createCommand);
            
            var createReceipt = new CreateReceiptsCommand
            {
                Total = 2000,
                FinancialProjectId = id,
                Title = "Title",
                BoughtAt = DateTime.Now
            };

            var receiptId = await SendAsync(createReceipt);

            var query = new GetAllFinancialProjectQuery(){ };

            var model = await SendAsync(query);

            
            model.Should().NotBeNull();
            model.FinancialProjects.First().Title.Should().Be(createCommand.Title);
            model.FinancialProjects.First().Receipts.First().Id.Should().Be(receiptId);
            model.FinancialProjects.First().Id.Should().Be(id);
            model.FinancialProjects.First().Receipts.First().Total.Should().Be(createReceipt.Total);
            model.FinancialProjects.First().Receipts.First().Title.Should().Be(createReceipt.Title);
            model.FinancialProjects.First().Receipts.First().BoughtAt.Should().BeCloseTo(DateTime.Now, 1000);

        }
    }
}