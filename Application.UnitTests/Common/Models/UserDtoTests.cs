using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Models
{
    public class UserDtoTests
    {

        [Test]
        public void HandleInitial_NameNotNull_ShouldReturnInitial()
        {
            var dto = new UserDto
            {
                Name = "John John"
            };

            dto.Name.Should().NotBeNull();
            dto.Name.Should().NotBeEmpty();
            dto.Name.Should().Be(dto.Name);
            dto.Initial.Should().Be("JJ");
        }

        [Test]
        public void HandleInitial_NameIsNull_ShouldReturnEmpty()
        {
            var dto = new UserDto();

            dto.Name.Should().BeNull();
            dto.Initial.Should().BeNullOrEmpty();
        }
        
        [Test]
        public void HandleInitial_NameIsEmpty_ShouldReturnEmpty()
        {
            var dto = new UserDto
            {
                Name = ""
            };

            dto.Name.Should().BeEmpty();
            dto.Initial.Should().BeNullOrEmpty();
        }

        [Test]
        public void HandleInitial_NameIsWhiteSpace_ShouldReturnEmpty()
        {
            var dto = new UserDto
            {
                Name = "     "
            };

            dto.Name.Should().BeNullOrWhiteSpace();
            dto.Initial.Should().BeNullOrEmpty();
        }
    }
}