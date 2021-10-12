using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Syhler.InformationGathering.Infrastructure.Entities;

namespace Syhler.InformationGathering.Infrastructure.Context
{
    public interface IApplicationDbContext
    {
        public DbSet<WebsiteEntity> WebsiteEntities { get; }
        public DbSet<YoutubeEntity> YoutubeEntities { get; }
        
        public DbSet<MusicEntity> MusicEntities { get; }
        
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());

        public Task<IDbContextTransaction> BeginTransactionAsync();

    }
}