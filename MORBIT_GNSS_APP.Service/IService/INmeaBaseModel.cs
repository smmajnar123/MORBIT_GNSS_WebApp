using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Service.IService
{
    public interface INmeaBaseModel
    {
        string LogLine { get; set; } 
        bool IsValid { get; set; }
        NmeaPrefix NmeaPrefix { get; set; }
        DateTime IstTime { get; set; } 
    }
}
