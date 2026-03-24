using Microsoft.Extensions.Logging;
using MORBIT_GNSS_APP.Core.Abstract;
using MORBIT_GNSS_APP.Core.Interface;
using MORBIT_GNSS_APP.Service.IService;
using MORBIT_GNSS_APP.Shared.Common;
using MORBIT_GNSS_APP.Shared.Constants;

namespace MORBIT_GNSS_APP.Core.Models
{
    public class SerialGnssServiceModel : NmeaLogParserBase, ISerialGnssServiceModel, IDisposable
    {
        private readonly ILogger<SerialGnssServiceModel> _logger;

        #region Constructor
        public SerialGnssServiceModel(ILogger<SerialGnssServiceModel> logger, ISerialGnssService serialGnssService, INmeaLogParseServiceModel nmeaLogParseServiceModel, IGnssEvent gnssEvent)
        {
            SerialGnssService = serialGnssService;
            NmeaSentenseParseServiceModel = nmeaLogParseServiceModel;
            _logger = logger;
            GnssEvent = gnssEvent;
            SerialGnssService.OnLineReceived += HandleLineReceived;
        }
        #endregion

        #region Properties
        public ISerialGnssService SerialGnssService { get; set; }
        public INmeaLogParseServiceModel NmeaSentenseParseServiceModel { get; set; }
        public bool IsConnected { get; set; }
        public IGnssEvent GnssEvent { get; set; }
        #endregion

        #region Methods
        private void HandleLineReceived(string line)
        {
            HandleLineSend(line);
            GnssEvent.RaiseLog(line);
        }
        private void HandleLineSend(string line)
        {
            NmeaSentenseParseServiceModel.NmeaEvent.LogSend(line);
            try
            {
                if (NmeaSentenseParseServiceModel.CurrentModel != null)
                {
                    if (IsValidNmeaLog(line, out Shared.Enums.GnssEnum.NmeaPrefix prefix))
                    {
                        GnssEvent.LogModelReceived(NmeaSentenseParseServiceModel.CurrentModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Error handling line send: {Message}"+ ex.Message);
            }
        }
        public void Connect(string port, int baud)
        {
            try
            {
                SerialGnssService.Connect(port, baud);
                IsConnected = SerialGnssService.IsConnected;
                GnssEvent.RaiseStatus(Constant.ConnectButtonText, false);
            }
            catch (Exception ex)
            {
                GnssEvent.RaiseStatus(ex.Message, true);
            }
        }
        public void Disconnect()
        {
            SerialGnssService.Disconnect();
            IsConnected = SerialGnssService.IsConnected;
            GnssEvent.RaiseStatus(Constant.DisconnectButtonText, true);
        }

        public void Dispose()
        {
            try
            {
                if (SerialGnssService.IsConnected)
                    SerialGnssService.Disconnect();

                if (NmeaSentenseParseServiceModel != null)
                    NmeaSentenseParseServiceModel.NmeaEvent.OnLogSend -= NmeaSentenseParseServiceModel.Send;

                if (SerialGnssService != null)
                    SerialGnssService.OnLineReceived -= HandleLineReceived;

                SerialGnssService?.Dispose();
                NmeaSentenseParseServiceModel?.Dispose();
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                Logger.Write("Dispose error: " + ex.Message);
            }
        }
        #endregion
    }
}
