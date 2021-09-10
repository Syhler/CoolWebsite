using System.Threading.Tasks;
using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Application.WebsiteInformationFeature.Common
{
    public interface IMusicInformationRepository
    {
        Task<bool> Insert(MusicInformation model);
    }
}