using MORBIT_GNSS_APP.Shared.Constants;
using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Core.Abstract
{
    public abstract class NmeaLogParserBase
    {
        public virtual bool IsValidNmeaLog(string line, out NmeaPrefix prefix)
        {
            prefix = NmeaPrefix.Unknown;
            if (!IsLineNotEmpty(line))
                return false;
            if (!HasValidStartSymbol(line))
                return false;
            if (!TryExtractPrefix(line, out prefix))
                return false;
            return prefix != NmeaPrefix.Unknown;
        }

        protected virtual bool IsLineNotEmpty(string line)
            => !string.IsNullOrWhiteSpace(line);

        protected virtual bool HasValidStartSymbol(string line)
            => line.StartsWith(Constant.StartSymbol);

        protected virtual bool TryExtractPrefix(string line, out NmeaPrefix prefix)
        {
            prefix = NmeaPrefix.Unknown;
            int commaIndex = line.IndexOf(',');
            if (commaIndex <= 1)
                return false;
            string prefixText = line.Substring(1, commaIndex - 1);
            if (!Enum.TryParse(prefixText, true, out prefix))
            {
                prefix = NmeaPrefix.Unknown;
            }
            return true;
        }
    }
}
