using MORBIT_GNSS_APP.Service.IService;
using MORBIT_GNSS_APP.Service.Models;
using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Service.Services
{
    public class NmeaLogParseService(INmeaBaseModel nmeaBaseModel, ILogParseFactory logParse) : INmeaLogParseService
    {
        private INmeaBaseModel baseModel = nmeaBaseModel;
        private readonly ILogParseFactory logParse = logParse;
        public INmeaBaseModel LogToModel(string line, NmeaPrefix nmeaPrefix)
        {
            baseModel.NmeaPrefix = nmeaPrefix;
            try
            {
                var parser = logParse.CreateLogParse(nmeaPrefix);
                if (parser.IsLogParse(line, ref baseModel))
                {
                    return baseModel;
                }
            }
            catch 
            {
                return new NmeaBaseModel
                {
                    LogLine = line,
                    IsValid = false,
                    NmeaPrefix = NmeaPrefix.Unknown,
                };
            }
            return baseModel;
        }
    }
}
