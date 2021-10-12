using System;
using System.Threading.Tasks;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;
using Syhler.InformationGathering.Infrastructure.Context;
using Syhler.InformationGathering.Infrastructure.Entities;

namespace Syhler.InformationGathering.Infrastructure.Repository
{
    public class YoutubeInformationRepository : IYoutubeInformationRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public YoutubeInformationRepository(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Insert(YoutubeInformation model)
        {
            await using var transaction = await _applicationDbContext.BeginTransactionAsync();

            var websiteEntity = WebsiteEntity.FromYoutubeDomain(model);
            var youtubeEntity = YoutubeEntity.FromDomain(model);
            
            try
            {
                websiteEntity.YoutubeEntities.Add(youtubeEntity);
                
                await _applicationDbContext.WebsiteEntities.AddAsync(websiteEntity);

                await _applicationDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}