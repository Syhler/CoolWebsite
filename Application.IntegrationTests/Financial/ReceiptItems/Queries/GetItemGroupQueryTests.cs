using System;
using System.Linq;
using System.Threading.Tasks;
using Application.IntegrationTests.Common;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries;
using CoolWebsite.Application.DatabaseAccess.Financials.ReceiptItems.Queries.Models;
using CoolWebsite.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Application.IntegrationTests.Financial.ReceiptItems.Queries
{
    using static Testing;
    
    public class GetItemGroupQueryTests : FinancialTestBase
    {
        [Test]
        public async Task Handle_Query_ShouldReturnObject()
        {
            var query = new GetItemGroupQuery();

            var enumsInArray = Enum.GetValues(typeof(ItemGroup))
                .Cast<ItemGroup>()
                .Select(x => new ItemGroupDto {Value = (int) x, Name = x.ToString()})
                .ToList();
            
            var enumsFromQuery = await SendAsync(query);


            enumsFromQuery.Should().BeEquivalentTo(enumsInArray);
        }
    }
}