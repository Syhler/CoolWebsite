using CoolWebsite.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoolWebsite.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TestEntity> TestEntities { get; set; }
        
    }
}
