namespace Syhler.InformationGathering.Domain.Entities
{
    public class YoutubeInformation : WebsiteInformation
    {
        public bool IsMusic { get; set; }
        public bool Genre { get; set; }
        public bool IsPlayingAndNotFocus { get; set; }
        public bool IsPlayingAndNotFocusNorCurrentPage { get; set; }
        public string Title { get; set; }
        public string UrlId { get; set; }
        public string ChannelName { get; set; }
        //public int Volume { get; set; } //For fun
        //public int VolumeComputer { get; set; } //For fun
    }
}