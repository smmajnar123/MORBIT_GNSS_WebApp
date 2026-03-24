using MORBIT_GNSS_APP.Service.IService;
using MORBIT_GNSS_APP.Service.Models;
using MORBIT_GNSS_APP.Shared.Helpers;
using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Service.Parse
{
    public class GsvLogParse : ILogParse
    {
        private static readonly Dictionary<string, List<GsvSatelliteInfo>> _cycleSatellites = new();

        public bool IsLogParse(string line, ref INmeaBaseModel baseModel)
        {
            if (string.IsNullOrWhiteSpace(line))
                return false;

            try
            {
                var parts = line.Split(',');

                var model = new GsvLogModel
                {
                    NmeaPrefix = baseModel.NmeaPrefix,
                    TotalMessages = ParseInt(parts, 1),
                    MessageNumber = ParseInt(parts, 2),
                    TotalSatellites = ParseInt(parts, 3),
                    LogLine = line
                };

                var prefixKey = model.NmeaPrefix.ToString();

                if (!_cycleSatellites.ContainsKey(prefixKey))
                    _cycleSatellites[prefixKey] = new List<GsvSatelliteInfo>();

                // Clear when first message in cycle
                if (model.MessageNumber == 1)
                    _cycleSatellites[prefixKey].Clear();

                // Extract talker (GP, GL, GA, GI, etc.)
                var talker = prefixKey.Substring(0, 2);

                for (int i = 4; i + 3 < parts.Length; i += 4)
                {
                    if (!int.TryParse(parts[i], out int prn) || prn == 0)
                        continue;

                    var gnssType = SignalHelper.GetGnss(talker, prn);

                    var sat = new GsvSatelliteInfo
                    {
                        Prn = prn,
                        Elevation = ParseInt(parts, i + 1),
                        Azimuth = ParseInt(parts, i + 2),
                        Snr = ParseSnr(parts[i + 3]),
                        GnssType = gnssType,
                        Country = SignalHelper.GetCountry(talker, prn),
                        GnssBandType = SignalHelper.GetBand(gnssType)
                    };

                    _cycleSatellites[prefixKey].Add(sat);
                }

                // Last message in cycle
                if (model.MessageNumber == model.TotalMessages)
                {
                    model.Satellites = _cycleSatellites[prefixKey]
                        .GroupBy(x => x.Prn)
                        .Select(x => x.First())
                        .ToList();

                    model.L1Count = model.Satellites.Count(x => x.GnssBandType == GnssBandType.L1);
                    model.L5Count = model.Satellites.Count(x => x.GnssBandType == GnssBandType.L5);
                    model.IsValid = true;

                    baseModel = model;
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private static int ParseInt(string[] parts, int index)
            => int.TryParse(parts.ElementAtOrDefault(index), out int v) ? v : 0;

        private static int ParseSnr(string raw)
        {
            var val = raw?.Split('*')[0];
            return int.TryParse(val, out int v) ? v : 0;
        }
    }
}