using MORBIT_GNSS_APP.Service.IService;
using MORBIT_GNSS_APP.Service.Models;

namespace MORBIT_GNSS_APP.Service.Parse
{
    public class GsaLogParse : ILogParse
    {
        public bool IsLogParse(string line, ref INmeaBaseModel baseModel)
        {
            try
            {
                var model = new GsaLogModel
                {
                    NmeaPrefix = baseModel.NmeaPrefix,
                    LogLine = line,
                };
                var parts = line.Split(',');
                if (parts.Length > 1)
                    model.Mode = parts[1];

                if (parts.Length > 2 && int.TryParse(parts[2], out var fix))
                    model.FixType = (Shared.Enums.GnssEnum.FixType)fix;

                // PRNs (3–14)
                for (int i = 3; i <= 14 && i < parts.Length; i++)
                {
                    if (int.TryParse(parts[i], out var prn))
                        model.SatellitePrns.Add(prn);
                }
                model.Pdop = double.TryParse(parts[15], out var pdop) ? pdop : 0.0;
                model.Hdop = double.TryParse(parts[16], out var hdop) ? hdop : 0.0;

                if (parts.Length > 17)
                {
                    model.Vdop = double.TryParse(parts[17].Split('*')[0], out var vdop) ? vdop : 0.0;
                }
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
