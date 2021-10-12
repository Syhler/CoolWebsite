using System;
using System.Data.Common;
using Syhler.InformationGathering.Domain.Common;
using Syhler.InformationGathering.Domain.Enums;
using Syhler.InformationGathering.Domain.ValueObject;

namespace Syhler.InformationGathering.Domain.Entities
{
    public class WebsiteInformation : AuditableDomain
    {
        public WebsiteInformation(WebsiteId id, string url, bool isCurrentPage, bool isInFocus, WebsiteInformationType type, DateTime timeVisited)
        {
            Id = id;
            Url = url;
            IsCurrentPage = isCurrentPage;
            IsInFocus = isInFocus;
            Type = type;
            TimeVisited = timeVisited;
        }

        public WebsiteId Id { get; private init; }
        public string Url { get; private init; }
        public bool IsCurrentPage { get; private init; }
        public bool IsInFocus { get; private init; }
        public DateTime TimeVisited { get; set; }
        
        public WebsiteInformationType Type { get; set; }

    }
}
