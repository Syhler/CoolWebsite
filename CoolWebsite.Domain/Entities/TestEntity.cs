using CoolWebsite.Domain.Common;

namespace CoolWebsite.Infrastructure.Persistence
{
    public class TestEntity : AudibleEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}