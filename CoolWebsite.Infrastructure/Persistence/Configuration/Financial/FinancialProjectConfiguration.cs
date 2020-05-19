using CoolWebsite.Domain.Entities.Financial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolWebsite.Infrastructure.Persistence.Configuration.Financial
{
    public class FinancialProjectConfiguration : IEntityTypeConfiguration<FinancialProject>
    {
        public void Configure(EntityTypeBuilder<FinancialProject> builder)
        {
            builder.Property(x => x.Description).HasMaxLength(9999);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Id).IsRequired();
        }
    }
}