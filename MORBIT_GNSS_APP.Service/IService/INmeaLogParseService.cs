using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Service.IService
{
    public interface INmeaLogParseService
    {
        INmeaBaseModel LogToModel(string line, NmeaPrefix nmeaPrefix);
    }
}
