namespace MORBIT_GNSS_APP.Shared.Enums
{
    public class GnssEnum
    {
        public enum NmeaPrefix
        {
            Unknown = 0,

            // -------- GPS (USA) --------
            GPRMC,
            GPGGA,
            GPGSA,
            GPGSV,
            GPGLL,
            GPVTG,

            // -------- NavIC / IRNSS (India) --------
            GIRMC,
            GIGGA,
            GIGSA,
            GIGSV,
            GIGLL,
            GIVTG,

            // -------- Mixed GNSS --------
            GNRMC,
            GNGGA,
            GNGSA,
            GNGSV,
            GNGLL,

            // -------- GLONASS --------
            GLGSV,
            GLGGA,
            GLRMC,
            GLGSA,

            // -------- Galileo --------
            GAGSV,
            GAGGA,
            GARMC,
            GAGSA,

            // -------- BeiDou --------
            GBGSV,
            GBGGA,
            GBRMC,
            GBGSA,

            // -------- QZSS --------
            GQGSV,

            // -------- Proprietary / Vendor --------
            PQTMANTENNASTATUS
        }
        public enum FixType
        {
            NoFix = 1,
            Fix2D = 2,
            Fix3D = 3
        }
        public enum  FixQuality
        {
            NoFix = 0,
            Fix = 1,
        }
        public enum GnssBandType
        {
            Unknown = 0,
            L1,   // GPS
            L5    // NavIC
        }
        public enum GnssType
        {
            GPS,
            GLONASS,
            Galileo,
            BeiDou,
            QZSS,
            NavIC,
            SBAS,
            Unknown,
            Mixed
        }
        public enum GnssCountry
        {
            USA,
            Russia,
            Europe,
            China,
            Japan,
            India,
            SBAS,
            Unknown
        }
    }
}
