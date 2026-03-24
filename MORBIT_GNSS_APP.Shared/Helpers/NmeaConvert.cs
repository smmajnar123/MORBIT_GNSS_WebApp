namespace MORBIT_GNSS_APP.Shared.Helpers
{
    public class NmeaConvert
    {
        public static double ToDecimal(string value, string dir)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;
            if (!double.TryParse(value, out var raw))
                return 0;
            double deg = Math.Floor(raw / 100);
            double min = raw - (deg * 100);
            double dec = deg + (min / 60);
            if (dir == "S" || dir == "W")
                dec *= -1;
            return dec;
        }
        public static string ConvertNmeaUtc(string utc)
        {
            if (string.IsNullOrWhiteSpace(utc) || utc.Length < 6)
                return string.Empty;

            int hour = int.Parse(utc.Substring(0, 2));
            int min = int.Parse(utc.Substring(2, 2));
            int sec = int.Parse(utc.Substring(4, 2));
            int ms = 0;
            var dot = utc.IndexOf('.');
            if (dot > 0)
            {
                var msStr = utc.Substring(dot + 1);
                ms = int.Parse(msStr.PadRight(3, '0'));
            }
            return $"{hour:D2}:{min:D2}:{sec:D2}.{ms:D3}";
        }

    }
}
