using System.Drawing;
using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Shared.Helpers
{
    public static class SignalHelper
    {
        // ================= COUNTRY COLOR =================
        public static Color GetCountryColor(GnssCountry country)
        {
            return country switch
            {
                GnssCountry.USA => Color.CornflowerBlue,   // bright blue
                GnssCountry.Russia => Color.LimeGreen,        // vivid green
                GnssCountry.Europe => Color.Gold,             // bright yellow
                GnssCountry.China => Color.MediumOrchid,     // purple
                GnssCountry.Japan => Color.DeepPink,         // hot pink
                GnssCountry.India => Color.Orange,           // vivid orange
                GnssCountry.SBAS => Color.Silver,           // metallic gray
                _ => Color.LightGray        // default for unknown
            };
        }

        // ================= GNSS TYPE COLOR (⭐ NEW) =================
        public static Color GetGnssColor(GnssType type)
        {
            return type switch
            {
                GnssType.GPS => Color.DodgerBlue,
                GnssType.GLONASS => Color.MediumSeaGreen,
                GnssType.Galileo => Color.Gold,
                GnssType.BeiDou => Color.MediumPurple,
                GnssType.QZSS => Color.DeepPink,
                GnssType.NavIC => Color.OrangeRed,
                GnssType.SBAS => Color.Silver,
                _ => Color.Gray
            };
        }

        // ================= MAIN PRODUCTION GNSS DETECTOR =================
        public static GnssType GetGnss(string nmeaPrefix, int prn)
        {
            if (nmeaPrefix.StartsWith("GI")) return GnssType.NavIC;

            if (nmeaPrefix.StartsWith("GN"))
            {
                // Use PRN ranges to detect GNSS
                var system = GetGnssByPrn(prn);
                if (system != GnssType.Unknown) return system;
            }

            // Prefix fallback
            if (nmeaPrefix.StartsWith("GQ")) return GnssType.QZSS;
            if (nmeaPrefix.StartsWith("GB") || nmeaPrefix.StartsWith("BD")) return GnssType.BeiDou;
            if (nmeaPrefix.StartsWith("GA")) return GnssType.Galileo;
            if (nmeaPrefix.StartsWith("GL")) return GnssType.GLONASS;
            if (nmeaPrefix.StartsWith("GP")) return GnssType.GPS;

            return GnssType.Unknown;
        }

        // ================= PURE PRN RANGE MAPPING =================
        public static GnssType GetGnssByPrn(int prn)
        {
            if (prn >= 1 && prn <= 32) return GnssType.GPS;
            if (prn >= 65 && prn <= 96) return GnssType.GLONASS;
            if (prn >= 120 && prn <= 158) return GnssType.SBAS;
            if (prn >= 193 && prn <= 199) return GnssType.QZSS;
            if (prn >= 201 && prn <= 237) return GnssType.BeiDou;
            if (prn >= 301 && prn <= 336) return GnssType.Galileo;
            if (prn >= 1 && prn <= 14) return GnssType.NavIC;

            return GnssType.Unknown;
        }

        // ================= COUNTRY MAPPING =================
        public static GnssCountry GetCountry(string nmeaPrefix, int prn)
        {
            var system = GetGnss(nmeaPrefix, prn);

            return system switch
            {
                GnssType.GPS => GnssCountry.USA,
                GnssType.GLONASS => GnssCountry.Russia,
                GnssType.Galileo => GnssCountry.Europe,
                GnssType.BeiDou => GnssCountry.China,
                GnssType.QZSS => GnssCountry.Japan,
                GnssType.NavIC => GnssCountry.India,
                GnssType.SBAS => GnssCountry.SBAS,
                _ => GnssCountry.Unknown
            };
        }

        // ================= BAND DETECTION =================
        public static GnssBandType GetBand(GnssType type)
        {
            return type switch
            {
                GnssType.GPS => GnssBandType.L1,
                GnssType.GLONASS => GnssBandType.L1,
                GnssType.Galileo => GnssBandType.L1,  // Galileo has L1 & L5, detect via PRN
                GnssType.BeiDou => GnssBandType.L1,   // B1/B2, detect via PRN
                GnssType.NavIC => GnssBandType.L5,    // NavIC L5 signals
                GnssType.QZSS => GnssBandType.L1,     // or L1/L5
                _ => GnssBandType.L1
            };
        }
       
    }
}