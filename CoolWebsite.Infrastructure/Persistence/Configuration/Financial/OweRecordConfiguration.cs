using CoolWebsite.Domain.Entities.Financial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolWebsite.Infrastructure.Persistence.Configuration.Financial
{
    public class OweRecordConfiguration : IEntityTypeConfiguration<OweRecord>
    {
        public void Configure(EntityTypeBuilder<OweRecord> builder)
        {
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.OwedUserId).IsRequired();
            builder.Property(x => x.FinancialProjectId).IsRequired();
        }
    }
}