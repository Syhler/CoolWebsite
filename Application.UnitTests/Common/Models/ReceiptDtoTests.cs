using System;
using System.Collections.Generic;
using System.Linq;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Models
{
    public class ReceiptDtoTests
    {

        [Test]
        public void HandleTotal_ValidItems_ShouldReturnTotal()
        {
            var dto = new ReceiptDto
            {
                Items = new List<ReceiptItemDto>
                {
                    new ReceiptItemDto
                    {
                        Price = 100,
                        Count = 10
                    },
                    new ReceiptItemDto
                    {
                        Price = 1,
                        Count = 1
                    }
                }
            };

            dto.Items.Should().NotBeNull();
            dto.Items.Count.Should().Be(2);
            dto.Total.Should().Be(100 * 10 + 1);
        }

        [Test]
        public void HandleTotal_ItemsIsNull_ShouldReturnZero()
        {
            var dto = new ReceiptDto
            {
                Items = null!
            };

            dto.Items.Should().BeNull();
            dto.Total.Should().Be(0);
        }

        [Test]
        public void HandleTotal_ItemsIsEmpty_ShouldReturnZero()
        {
            var dto = new ReceiptDto
            {
                Items = new List<ReceiptItemDto>()
            };

            dto.Items.Should().NotBeNull();
            dto.Items.Count.Should().Be(0);
            dto.Total.Should().Be(0);
        }

        [Test]
        public void HandleDaysDaysSinceLastVisit_ValidDate_ShouldReturnCorrectNumberOfDays()
        {
            var dto = new ReceiptDto
            {
                DateVisited = DateTime.Now.AddDays(-5)
            };

            dto.DateVisited.Should().BeCloseTo(dto.DateVisited, 10000);
            
            dto.DaysSinceLastVisit.Should().Be(5);
        }
        
        [Test]
        public void HandleDaysDaysSinceDeleted_ValidDate_ShouldReturnCorrectNumberOfDays()
        {
            var dto = new ReceiptDto
            {
                Deleted = DateTime.Now.AddDays(-5)
            };

            dto.Deleted.Should().BeCloseTo(dto.Deleted.GetValueOrDefault(), 10000);
            
            dto.DaysSinceDeleted.Should().Be(5);
        }

        [Test]
        public void HandleCurrentUserSum_ValidFields_ShouldReturnCorrectNumber()
        {
            var dto = new ReceiptDto
            {
                CurrentUserId = "detmig",
                Items = new List<ReceiptItemDto>
                {
                    new ReceiptItemDto
                    {
                        Count = 100,
                        Price = 100,
                        Users = new List<UserDto>
                        {
                            new UserDto
                            {
                                Id = "detmig"
                            },
                            new UserDto
                            {
                                Id = "nah"
                            }
                        }
                    }
                }
            };

            dto.Items.Should().NotBeNull();
            dto.CurrentUserOwed.Should().Be(dto.Total / dto.Items.First().Users.Count);
        }

        [Test]
        public void HandleCurrentUserSum_UsersNull_ShouldReturnZero()
        {
            var dto = new ReceiptDto
            {
                CurrentUserId = "detmig",
                Items = new List<ReceiptItemDto>
                {
                    new ReceiptItemDto
                    {
                        Count = 100,
                        Price = 100,
                    }
                }
            };

            dto.Items.Should().NotBeNull();
            dto.CurrentUserOwed.Should().Be(0);
        }
        
    }
}