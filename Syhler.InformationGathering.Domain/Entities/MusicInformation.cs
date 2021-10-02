namespace Syhler.InformationGathering.Domain.Entities
{
    public class MusicInformation : WebsiteInformation
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
    }
}