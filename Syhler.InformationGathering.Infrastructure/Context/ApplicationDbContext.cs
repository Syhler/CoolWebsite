using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Syhler.InformationGathering.Application.Common.Interface;
using Syhler.InformationGathering.Infrastructure.Entities;
using Syhler.InformationGathering.Infrastructure.Entities.Common;

namespace Syhler.InformationGathering.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDateTimeService _dateTimeService;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTimeService) : base(options)
        {
            _dateTimeService = dateTimeService;
        }

        public DbSet<WebsiteEntity> WebsiteEntities { get; private set; } = null!;
        public DbSet<YoutubeEntity> YoutubeEntities { get; private set; } = null!;
        public DbSet<MusicEntity> MusicEntities { get; private set; } = null!;

        public Task<IDbContextTransaction> BeginTransactionAsync() => Database.BeginTransactionAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = Guid.Empty.ToString(); //ADMIN FOR NOW
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTimeService.Now;
                        entry.Entity.LastModifiedBy = Guid.Empty.ToString(); //ADMIN FOR NOW
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}