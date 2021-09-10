using System;
using System.Threading.Tasks;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Infrastructure.Repository
{
    public class WebsiteInformationRepository : IWebsiteInformationRepository
    {
        public Task<bool> Insert(WebsiteInformation model)
        {
            Console.WriteLine("Insert: " + model.Id);
            return Task.FromResult(true);
        }
    }
}