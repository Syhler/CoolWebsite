using CoolWebsite.Domain.Entities.Financial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoolWebsite.Infrastructure.Persistence.Configuration.Financial
{
    public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
    {
        public void Configure(EntityTypeBuilder<Receipt> builder)
        {
            builder.Property(x => x.Location).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(255);
            builder.Property(x => x.DateVisited).IsRequired();
            builder.Property(x => x.FinancialProjectId).IsRequired();
            builder.Property(x => x.Id).IsRequired();
            
        }
    }
}