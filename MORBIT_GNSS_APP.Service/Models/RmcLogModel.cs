namespace MORBIT_GNSS_APP.Service.Models
{
    public class RmcLogModel : NmeaBaseModel
    {
        public string UtcTime { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public double Speed { get; set; }
        public double Course { get; set; }
        public string Date { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
