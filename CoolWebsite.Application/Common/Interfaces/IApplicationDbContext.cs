using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Domain.Entities;
using CoolWebsite.Domain.Entities.Financial;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<FinancialProject> FinancialProjects { get; set; }
        
        DbSet<Receipt> Receipts { get; set; }

        DbSet<ReceiptItem> ReceiptItems { get; set; }
        
        DbSet<FinancialProjectApplicationUser> FinancialProjectApplicationUsers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        string UserId { get; set; }

    }
}
