using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Exceptions;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Common;
using CoolWebsite.Domain.Entities.Financial;
using CoolWebsite.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Infrastructure.Persistence
{
    
    public class MySqlApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IApplicationDbContext
    {
        
        private readonly IDateTime _dateTime;
        public string? UserId { get; set; }

        public MySqlApplicationDbContext(DbContextOptions<MySqlApplicationDbContext> options,
            IDateTime dateTime) : base(options)
        {
            _dateTime = dateTime;
        }

        public DbSet<FinancialProject> FinancialProjects { get; set; } = null!;

        public DbSet<FinancialProjectApplicationUser> FinancialProjectApplicationUsers { get; set; } = null!;

        public DbSet<ApplicationUserReceiptItem> ApplicationUserReceiptItems { get; set; } = null!;

        public DbSet<Transaction> Transactions { get; set; } = null!;

        public DbSet<OweRecord> OweRecords { get; set; } = null!;

        public DbSet<ReceiptItem> ReceiptItems { get; set; } = null!;

        public DbSet<Receipt> Receipts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<FinancialProjectApplicationUser>()
                .HasKey(x => new {x.UserId, x.FinancialProjectId});

            builder.Entity<ApplicationUserReceiptItem>()
                .HasKey(x => new {x.ApplicationUserId, x.ReceiptItemId});

            builder.Entity<OweRecord>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Transaction>()
                .HasOne(x => x.FromUser)
                .WithMany(x => x.IncomingTransactions)
                .HasForeignKey(x => x.FromUserId)
                .HasPrincipalKey(x => x.Id);


            builder.Entity<Transaction>()
                .HasOne(x => x.ToUser)
                .WithMany(x => x.OutgoingTransactions)
                .HasForeignKey(x => x.ToUserId).OnDelete(DeleteBehavior.NoAction)
                .HasPrincipalKey(x => x.Id);
            ;


            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<AudibleEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (UserId == null)
                        {
                            throw new IdentityCurrentUserIdNotSet();
                        }

                        entry.Entity.CreatedBy = UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Modified:
                        if (UserId == null)
                        {
                            throw new IdentityCurrentUserIdNotSet();
                        }

                        entry.Entity.LastModified = _dateTime.Now;
                        entry.Entity.LastModifiedBy = UserId;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}