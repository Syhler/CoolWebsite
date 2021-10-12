using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;
using Syhler.InformationGathering.Infrastructure.Context;
using Syhler.InformationGathering.Infrastructure.Entities;

namespace Syhler.InformationGathering.Infrastructure.Repository
{
    public class WebsiteInformationRepository : IWebsiteInformationRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public WebsiteInformationRepository(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task<bool> InsertAsync(WebsiteInformation model)
        {
            Console.WriteLine("Insert: " + model.Id);
            return Task.FromResult(true);
        }

        public async Task<bool> InsertRangeAsync(List<WebsiteInformation> collection)
        {
            await using var transaction = await _applicationDbContext.BeginTransactionAsync();

            var collectionOfEntities = collection.Select(WebsiteEntity.FromWebsiteDomain);

            try
            {
                await _applicationDbContext.WebsiteEntities.AddRangeAsync(collectionOfEntities);
                await _applicationDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}