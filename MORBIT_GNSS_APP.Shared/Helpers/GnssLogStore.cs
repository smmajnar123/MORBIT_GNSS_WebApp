namespace MORBIT_GNSS_APP.Shared.Helpers
{
    public static class GnssLogStore
    {
        private static readonly List<string> _logs = [];

        public static void Add(string log)
        {
            _logs.Add(log);

            // Keep last 100 logs only
            if (_logs.Count > 1000)
                _logs.RemoveAt(0);
        }

        public static List<string> Get()
        {
           return [.. _logs];
        }
    }
}
