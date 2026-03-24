using MORBIT_GNSS_APP.Service.IService;
using MORBIT_GNSS_APP.Service.Models;
using MORBIT_GNSS_APP.Shared.Enums;
using MORBIT_GNSS_APP.Shared.Helpers;

namespace MORBIT_GNSS_APP.Service.Parse
{
    public class RmcLogParse : ILogParse
    {
        public bool IsLogParse(string line, ref INmeaBaseModel baseModel)
        {
            try
            {
                var model = new RmcLogModel
                {
                    LogLine = line,
                    IsValid = false,
                    NmeaPrefix = baseModel?.NmeaPrefix ?? GnssEnum.NmeaPrefix.Unknown,
                };
                var p = line.Split(',');
                model.NmeaPrefix = baseModel?.NmeaPrefix ?? GnssEnum.NmeaPrefix.Unknown;
                model.UtcTime = NmeaConvert.ConvertNmeaUtc(p[1]);
                model.Status = p[2];
                model.Latitude = NmeaConvert.ToDecimal(p[3], p[4]);
                model.Longitude = NmeaConvert.ToDecimal(p[5], p[6]);
                model.Speed = double.TryParse(p[7], out var speed) ? speed : 0;
                model.Course = double.TryParse(p[8], out var course) ? course : 0;
                model.Date = DateTime.TryParseExact(p[9],"ddMMyy",null,
                System.Globalization.DateTimeStyles.None,out var dt) ? dt.ToString("dd-MM-yyyy"): "";
                model.IsValid = true;
                baseModel = model;
                return true;
            }
            catch
            {
                baseModel = null!;
                return false;
            }
        }
    }
}
