using System.Collections.Generic;
using System.Threading.Tasks;
using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Application.WebsiteInformationFeature.Common
{
    public interface IWebsiteInformationRepository
    {
        Task<bool> InsertAsync(WebsiteInformation model);
        Task<bool> InsertRangeAsync(List<WebsiteInformation> collection);
    }
}