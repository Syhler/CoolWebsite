using System;

namespace Syhler.InformationGathering.Domain.Common
{
    public class AuditableDomain
    {
        public string? CreatedByInSystem { get; set; }

        public DateTime? LastModifiedInSystem { get; set; }

        public string? LastModifiedByInSystem { get; set; }
    }
}