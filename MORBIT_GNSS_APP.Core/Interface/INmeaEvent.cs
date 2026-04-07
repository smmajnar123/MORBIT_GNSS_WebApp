namespace MORBIT_GNSS_APP.Core.Interface
{
    public interface INmeaEvent 
    {
        event Action<string>? OnLogSend;
        void LogSend(string line);
    }
}
