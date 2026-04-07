namespace MORBIT_GNSS_APP.Service.Models
{
    public class GgaLogModel : NmeaBaseModel
    {
        public Shared.Enums.GnssEnum.FixQuality FixQuality { get; set; }
        public int SatelliteCount { get; set; }
        public double Hdop { get; set; }
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? UtcTime { get; set; }
    }
}
