using CoolWebsite.Domain.Entities.Financial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolWebsite.Infrastructure.Persistence.Configuration.Financial
{
    public class ReceiptItemConfiguration : IEntityTypeConfiguration<ReceiptItem>
    {
        public void Configure(EntityTypeBuilder<ReceiptItem> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Count).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.ItemGroup).IsRequired();
        }
    }
}