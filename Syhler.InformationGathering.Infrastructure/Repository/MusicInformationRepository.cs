using System;
using System.Threading.Tasks;
using Syhler.InformationGathering.Application.WebsiteInformationFeature.Common;
using Syhler.InformationGathering.Domain.Entities;
using Syhler.InformationGathering.Infrastructure.Context;
using Syhler.InformationGathering.Infrastructure.Entities;

namespace Syhler.InformationGathering.Infrastructure.Repository
{
    public class MusicInformationRepository : IMusicInformationRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public MusicInformationRepository(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Insert(MusicInformation model)
        {
            await using var transaction = await _applicationDbContext.BeginTransactionAsync();

            var websiteEntity = WebsiteEntity.FromMusicDomain(model);
            var musicEntity = MusicEntity.FromDomain(model);
            
            try
            {
                websiteEntity.MusicEntities.Add(musicEntity);
                
                await _applicationDbContext.WebsiteEntities.AddAsync(websiteEntity);
                
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