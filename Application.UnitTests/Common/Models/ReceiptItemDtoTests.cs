using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Models
{
    public class ReceiptItemDtoTests
    {
        
        [Test]
        public void HandleTotal_Valid_ShouldReturnCorrectNumber()
        {
            var dto = new ReceiptItemDto
            {
                Price = 100,
                Count = 100
            };

            dto.Price.Should().Be(dto.Price);
            dto.Count.Should().Be(dto.Count);
            dto.Total.Should().Be(dto.Price * dto.Count);
        }
    }
}