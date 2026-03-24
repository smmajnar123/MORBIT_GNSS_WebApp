using MORBIT_GNSS_APP.Service.IService;
using MORBIT_GNSS_APP.Service.Models;
using MORBIT_GNSS_APP.Shared.Enums;
using MORBIT_GNSS_APP.Shared.Helpers;

namespace MORBIT_GNSS_APP.Service.Parse
{
    public class GgaLogParse : ILogParse
    {
        public bool IsLogParse(string line, ref INmeaBaseModel baseModel)
        {
            try
            {
                GgaLogModel model = new();
                var p = line.Split(',');
                model.NmeaPrefix = baseModel.NmeaPrefix;
                model.LogLine = line;
                model.UtcTime = NmeaConvert.ConvertNmeaUtc(p[1]);

                //  coordinate convert
                model.Latitude = NmeaConvert.ToDecimal(p[2], p[3]);
                model.Longitude = NmeaConvert.ToDecimal(p[4], p[5]);

                //  fix quality
                int.TryParse(p[6], out int fix);
                model.FixQuality = (GnssEnum.FixQuality)fix;

                //  satellite count
                int.TryParse(p[7], out int sat);
                model.SatelliteCount = sat;
                //  HDOP
                double.TryParse(p[8], out double hdop);
                model.Hdop = hdop;
                // altitude
                double.TryParse(p[9], out double alt);
                model.Altitude = alt;
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
