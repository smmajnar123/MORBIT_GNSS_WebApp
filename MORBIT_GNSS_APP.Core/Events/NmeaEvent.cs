using MORBIT_GNSS_APP.Core.Interface;

namespace MORBIT_GNSS_APP.Core.Events
{
    public class NmeaEvent : INmeaEvent
    {
        public event Action<string>? OnLogSend;

        public void LogSend(string line)
        {
            OnLogSend?.Invoke(line);
        }
    }
}
