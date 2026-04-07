using MORBIT_GNSS_APP.Service.IService;
using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Service.Models
{
    public class NmeaBaseModel : INmeaBaseModel
    {
        public string LogLine { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public NmeaPrefix NmeaPrefix { get; set; }
        public DateTime IstTime { get; set; } = DateTime.Now;
    }
}
