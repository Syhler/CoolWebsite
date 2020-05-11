using CoolWebsite.Domain.Entities;
using CoolWebsite.Domain.Entities.Financial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolWebsite.Infrastructure.Persistence.Configuration
{
    public class TestEntityConfiguration : IEntityTypeConfiguration<FinancialProject>
    {
        /*
        public void Configure(EntityTypeBuilder<TestEntity> builder)
        {
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        }
        */

        public void Configure(EntityTypeBuilder<FinancialProject> builder)
        {
            
        }
    }
}