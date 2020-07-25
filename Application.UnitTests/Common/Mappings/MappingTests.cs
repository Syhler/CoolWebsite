using System;
using AutoMapper;
using CoolWebsite.Application.Common.Mapping;
using CoolWebsite.Application.DatabaseAccess.Common.Transaction.Queries.Models;
using CoolWebsite.Application.DatabaseAccess.Financials.FinancialProjects.Queries.GetFinancialProjects.Models;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;
using CoolWebsite.Domain.Enums;
using NUnit.Framework;

namespace Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DTOMappingProfile>();

            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        [TestCase(typeof(FinancialProject), typeof(FinancialProjectDto))]
        [TestCase(typeof(Receipt), typeof(ReceiptDto))]
        [TestCase(typeof(ReceiptItem), typeof(ReceiptItemDto))]
        [TestCase(typeof(ApplicationUser), typeof(UserDto))]
        [TestCase(typeof(Transaction), typeof(TransactionDto))]
        [TestCase(typeof(TransactionType), typeof(TransactionTypeDto))]

        public void Handle_SupportMapping_ShouldMapFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }

       
    }
}