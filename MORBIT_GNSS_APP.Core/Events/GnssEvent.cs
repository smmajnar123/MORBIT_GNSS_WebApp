using MORBIT_GNSS_APP.Core.Interface;
using MORBIT_GNSS_APP.Service.IService;

namespace MORBIT_GNSS_APP.Core.Events
{
    public class GnssEvent : IGnssEvent
    {
        #region events
        public event Action<string>? OnLogReceived;
        public event Action<string, bool>? OnStatusMessage;
        public event Action<INmeaBaseModel>? OnLogModelReceived;
        #endregion

        #region event raisers
        public void RaiseLog(string line)
        {
            OnLogReceived?.Invoke(line);
        }

        public void RaiseStatus(string message, bool isError)
        {
            OnStatusMessage?.Invoke(message, isError);
        }
        public void LogModelReceived(INmeaBaseModel nmeaBaseModel)
        {
            if (nmeaBaseModel != null)
            {
                OnLogModelReceived?.Invoke(nmeaBaseModel);
            }
        }
        #endregion
    }
}
