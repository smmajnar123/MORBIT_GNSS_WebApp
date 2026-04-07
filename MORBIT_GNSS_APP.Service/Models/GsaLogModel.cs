using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Service.Models
{
    public class GsaLogModel : NmeaBaseModel
    {
        public string Mode { get; set; } = string.Empty;
        public FixType FixType { get; set; }
        public List<int> SatellitePrns { get; set; } = [];
        public double Pdop { get; set; }
        public double Hdop { get; set; }
        public double Vdop { get; set; }
        public double Accurracy { get; set; }
    }
}
