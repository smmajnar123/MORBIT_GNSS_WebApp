using Microsoft.Extensions.Logging;
using MORBIT_GNSS_APP.Core.Abstract;
using MORBIT_GNSS_APP.Core.Interface;
using MORBIT_GNSS_APP.DataAccessLayer.Models;
using MORBIT_GNSS_APP.Repository.IRepository;
using MORBIT_GNSS_APP.Service.IService;
using MORBIT_GNSS_APP.Service.Models;
using MORBIT_GNSS_APP.Shared.Common;
using MORBIT_GNSS_APP.Shared.Constants;
using static MORBIT_GNSS_APP.Shared.Enums.GnssEnum;

namespace MORBIT_GNSS_APP.Core.Models
{
    public class SerialGnssServiceModel : NmeaLogParserBase, ISerialGnssServiceModel, IDisposable
    {
        private readonly ILogger<SerialGnssServiceModel> _logger;
        public IGnssDataRepository GnssDataRepository { get; set; }

        #region Constructor
        public SerialGnssServiceModel(ILogger<SerialGnssServiceModel> logger, ISerialGnssService serialGnssService, INmeaLogParseServiceModel nmeaLogParseServiceModel, IGnssEvent gnssEvent)
        {
            SerialGnssService = serialGnssService;
            NmeaSentenseParseServiceModel = nmeaLogParseServiceModel;
            _logger = logger;
            GnssEvent = gnssEvent;
            SerialGnssService.OnLineReceived += HandleLineReceived;
            NmeaSentenseParseServiceModel.NmeaEvent.OnLogSend += NmeaSentenseParseServiceModel.Send;
            GnssEvent.OnLogModelReceived += Gnss_OnLogModelReceived;
        }
        #endregion

        #region Properties
        public ISerialGnssService SerialGnssService { get; set; }
        public INmeaLogParseServiceModel NmeaSentenseParseServiceModel { get; set; }
        public bool IsConnected { get; set; }
        public IGnssEvent GnssEvent { get; set; }
        public List<GnssDataModel> GnssDataModels { get; set; } = [];
        #endregion

        #region Methods
        private void HandleLineReceived(string line)
        {
            HandleLineSend(line);
        }
        private void Gnss_OnLogModelReceived(INmeaBaseModel model)
        {
            _logger.LogInformation($"Received GNSS Log Model: {model.LogLine}");
            if (model == null) return;
            try
            {
                switch (model.NmeaPrefix)
                {
                    case NmeaPrefix.GNGGA:
                        {
                            if (model is GgaLogModel gga)
                            {
                                GnssDataModels.Add(new GnssDataModel
                                {
                                    GnssNmeaModel = new
                                    {
                                        LogLine = gga.LogLine,
                                        IsValid = gga.IsValid,
                                        NmeaPrefix = gga.NmeaPrefix.ToString(),
                                        IstTime = gga.IstTime,

                                        FixQuality = gga.FixQuality,
                                        SatelliteCount = gga.SatelliteCount,
                                        Hdop = gga.Hdop,
                                        Altitude = gga.Altitude,
                                        Latitude = gga.Latitude,
                                        Longitude = gga.Longitude,
                                        UtcTime = gga.UtcTime
                                    }
                                });

                            }
                            break;
                        }
                    case NmeaPrefix.GNRMC:
                        {
                            if (model is RmcLogModel rmc)
                            {
                                GnssDataModels.Add(new GnssDataModel
                                {
                                    GnssNmeaModel = new
                                    {
                                        LogLine = rmc.LogLine,
                                        IsValid = rmc.IsValid,
                                        NmeaPrefix = rmc.NmeaPrefix.ToString(),
                                        IstTime = rmc.IstTime,
                                        Latitude = rmc.Latitude,
                                        Longitude = rmc.Longitude,
                                    }
                                });
                            }
                            break;
                        }
                    case NmeaPrefix.GNGSA:
                        {
                            if (model is GsaLogModel gsa)
                            {
                                GnssDataModels.Add(new GnssDataModel
                                {
                                    GnssNmeaModel = new
                                    {
                                        LogLine = gsa.LogLine,
                                        IsValid = gsa.IsValid,
                                        NmeaPrefix = gsa.NmeaPrefix.ToString(),
                                        IstTime = gsa.IstTime,
                                        Accurracy = gsa.Hdop * 5.0,
                                        FixType = gsa.FixType,
                                        Pdop = gsa.Pdop,
                                        Hdop = gsa.Hdop,
                                        Vdop = gsa.Vdop,
                                    }
                                });
                            }
                            break;
                        }
                    case NmeaPrefix.GPGSV:
                    case NmeaPrefix.GLGSV:
                    case NmeaPrefix.GAGSV:
                    case NmeaPrefix.GBGSV:
                    case NmeaPrefix.GIGSV:
                    case NmeaPrefix.GQGSV:
                        {
                            if (model is GsvLogModel gsv)
                            {
                                GnssDataModels.Add(new GnssDataModel
                                {
                                    GnssNmeaModel = new
                                    {
                                        LogLine = gsv.LogLine,
                                        IsValid = gsv.IsValid,
                                        NmeaPrefix = gsv.NmeaPrefix.ToString(),
                                        IstTime = gsv.IstTime,

                                        TotalMessages = gsv.TotalMessages,
                                        MessageNumber = gsv.MessageNumber,
                                        L1Count = gsv.L1Count,
                                        L5Count = gsv.L5Count,
                                        TotalSatellites = gsv.TotalSatellites,
                                    }
                                });
                            }
                            break;
                        }
                }

                if (GnssDataModels.Count > 0)
                {
                    if (GnssDataRepository!=null)
                    {
                        foreach (var dataModel in GnssDataModels)
                        {
                            var entity = new GnssData
                            {
                                Id = Guid.NewGuid(),
                                GnssNmeaJson = System.Text.Json.JsonSerializer.Serialize(dataModel.GnssNmeaModel),
                                CreatedAt = DateTime.UtcNow
                            };
                            GnssDataRepository.AddAsync(entity);
                        }
                        GnssDataModels.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing GNSS log model: {ex.Message}");
            }
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
