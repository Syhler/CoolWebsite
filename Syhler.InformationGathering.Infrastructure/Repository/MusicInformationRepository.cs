using System;
using System.Threading.Tasks;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Infrastructure.Repository
{
    public class MusicInformationRepository : IMusicInformationRepository
    {
        public Task<bool> Insert(MusicInformation model)
        {
            Console.WriteLine("Insert: " + model.Id);
            return Task.FromResult(true);
        }
    }
}