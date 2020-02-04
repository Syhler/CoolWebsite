using System;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using CoolWebsite.Application.Common.Interfaces;
using CoolWebsite.Domain.Common;
using CoolWebsite.Domain.Entities;
using CoolWebsite.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IApplicationDbContext
    {
        private ICurrentUserService _currentUserService;
        private IDateTime _dateTime;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
            IDateTime dateTime, ICurrentUserService currentUserService, IHttpContextAccessor accessor) : base(options)
        {
            _dateTime = dateTime;
            _currentUserService = currentUserService;
            var userId = accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            

        }

        public DbSet<TestEntity> TestEntities { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {

            foreach (var entry in ChangeTracker.Entries<AudibleEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserID;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.Now;
                        entry.Entity.LastModifiedBy = _currentUserService.UserID;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}