using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TestEntity> TestEntities { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        public string UserId { get; set; }

    }
}
