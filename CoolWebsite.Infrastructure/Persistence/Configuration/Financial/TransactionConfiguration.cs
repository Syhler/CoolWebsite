using CoolWebsite.Domain.Entities.Financial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolWebsite.Infrastructure.Persistence.Configuration.Financial
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.ToUserId).IsRequired();
            builder.Property(x => x.FromUserId).IsRequired();
            builder.Property(x => x.TransactionType).IsRequired();
        }
    }
}