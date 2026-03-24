namespace MORBIT_GNSS_APP.Shared.Common
{
    public static class Logger
    {
        private static readonly object _lock = new();
        private static string LogDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static string LogFilePath => Path.Combine(LogDirectory, $"gnss.txt");
        public static void Write(string message)
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                    Directory.CreateDirectory(LogDirectory);
                var line = $"{DateTime.Now:HH:mm:ss} | {message}";
                lock (_lock)
                {
                    File.AppendAllText(LogFilePath, line + Environment.NewLine);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
    }
}
