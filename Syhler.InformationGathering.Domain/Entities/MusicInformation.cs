namespace Syhler.InformationGathering.Domain.Entities
{
    public class MusicInformation : WebsiteInformation
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        //public int Volume { get; set; } //For fun
        //public int VolumeComputer { get; set; } //For fun
        public string Genre { get; set; }
    }
}