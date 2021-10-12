using System;

namespace Syhler.InformationGathering.Infrastructure.Entities.Common
{
    public class AuditableEntity
    {
        public string CreatedBy { get; set; } = null!;

        public DateTime? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
        
    }
}