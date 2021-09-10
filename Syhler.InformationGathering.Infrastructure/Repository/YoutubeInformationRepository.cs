using System;
using System.Threading.Tasks;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Infrastructure.Repository
{
    public class YoutubeInformationRepository : IYoutubeInformationRepository
    {
        public Task<bool> Insert(YoutubeInformation model)
        {
            Console.WriteLine("Insert: " + model.Id);
            return Task.FromResult(true);
        }
    }
}