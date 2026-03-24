using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;
namespace MORBIT_GNSS_APP.Service.IService
{
    public interface ILogParseFactory
    {
        ILogParse CreateLogParse(NmeaPrefix prefix);
    }
}
