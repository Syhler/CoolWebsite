using System;
using System.ComponentModel.DataAnnotations.Schema;
using CoolWebsite.Domain.Entities.Identity;

namespace CoolWebsite.Domain.Common
{
    public class AudibleEntity
    {
        [ForeignKey("CreatedBy")]
        public ApplicationUser CreatedByUser { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}