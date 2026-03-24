using MORBIT_GNSS_APP.Service.IService;

namespace MORBIT_GNSS_APP.Core.Interface
{
    public interface IGnssEvent
    {
        event Action<string>? OnLogReceived;
        event Action<string, bool>? OnStatusMessage;
        event Action<INmeaBaseModel>? OnLogModelReceived;
        void RaiseLog(string line);
        void RaiseStatus(string message, bool isError);
        void LogModelReceived(INmeaBaseModel nmeaBaseModel);
    }
}
