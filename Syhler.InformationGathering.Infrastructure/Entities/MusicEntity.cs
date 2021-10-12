using Syhler.InformationGathering.Domain.Entities;

namespace Syhler.InformationGathering.Infrastructure.Entities
{
    public class MusicEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Artist { get; set; } = null!;

        public int WebsiteEntityId { get; set; }
        public WebsiteEntity WebsiteEntity { get; set; } = null!;

        public static MusicEntity FromDomain(MusicInformation model)
        {
            return new MusicEntity
            {
                Title = model.Title,
                Artist = model.Artist
            };
        }
    }
}