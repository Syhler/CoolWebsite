using Syhler.InformationGathering.Domain.Common;
using Syhler.InformationGathering.Domain.Enums;

namespace Syhler.InformationGathering.Domain.Entities
{
    public class WebsiteInformation : AuditableEntity
    {
        public int Id { get; init; }
        public string Url { get; init; } = null!; //Cant be null
        public bool IsCurrentPage { get; set; }
        public bool IsInFocus { get; set; }
        public WebsiteInformationType Type { get; set; }

    }
}
