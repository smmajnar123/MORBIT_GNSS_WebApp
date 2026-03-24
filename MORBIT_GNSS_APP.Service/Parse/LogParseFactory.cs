using MORBIT_GNSS_APP.Service.IService;
using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Service.Parse
{
    public class LogParseFactory : ILogParseFactory
    {
        public ILogParse CreateLogParse(NmeaPrefix prefix)
        {
            return prefix switch
            {
                NmeaPrefix.GNRMC => new RmcLogParse(),
                NmeaPrefix.GNGGA => new GgaLogParse(),
                NmeaPrefix.GNGSA => new GsaLogParse(),

                NmeaPrefix.GPGSV or
                NmeaPrefix.GLGSV or
                NmeaPrefix.GAGSV or
                NmeaPrefix.GBGSV or
                NmeaPrefix.GIGSV or
                NmeaPrefix.GQGSV or
                NmeaPrefix.GNGSV => new GsvLogParse(),

                _ => throw new NotSupportedException("Invalid prefix")
            };
        }
    }
}
