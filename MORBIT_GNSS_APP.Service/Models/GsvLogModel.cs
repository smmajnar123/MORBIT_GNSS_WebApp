using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Service.Models
{
    public class GsvLogModel : NmeaBaseModel
    {
        public int TotalMessages { get; set; }
        public int MessageNumber { get; set; }
        public int TotalSatellites { get; set; }
        public int L1Count { get; set; }
        public int L5Count { get; set; }
        public List<GsvSatelliteInfo> Satellites { get; set; } = [];
    }
    public class GsvSatelliteInfo
    {
        public int Prn { get; set; }
        public int Elevation { get; set; }
        public int Azimuth { get; set; }
        public double Snr { get; set; }
        public double CurrentSnr { get; set; }

        public GnssType GnssType { get; set; }
        public GnssCountry Country { get; set; }
        public GnssBandType GnssBandType { get; set; }

        public bool IsUsedInFix { get; set; }

        // ⭐ NEW
        public DateTime LastSeen { get; set; } = DateTime.Now;
        public float FadeAlpha { get; set; } = 1f;
    }

}
