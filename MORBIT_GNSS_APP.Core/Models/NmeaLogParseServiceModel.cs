using MORBIT_GNSS_APP.Core.Abstract;
using MORBIT_GNSS_APP.Core.Interface;
using MORBIT_GNSS_APP.Service.IService;
namespace MORBIT_GNSS_APP.Core.Models
{
    public class NmeaLogParseServiceModel(INmeaLogParseService nmeaLogParseService, INmeaEvent nmeaEvent, INmeaBaseModel nmeaBaseModel) : NmeaLogParserBase, INmeaLogParseServiceModel
    {
        #region Events
        public INmeaEvent NmeaEvent => nmeaEvent;
        #endregion

        #region Properties
        public INmeaLogParseService? NmeaSentenseParseService { get; set; } = nmeaLogParseService;
        public INmeaBaseModel CurrentModel { get; set; } = nmeaBaseModel;
        #endregion
       
        #region Methods
        public void Send(string line)
        {
            if (IsValidNmeaLog(line, out Shared.Enums.GnssEnum.NmeaPrefix prefix))
            {
                CurrentModel = NmeaSentenseParseService!.LogToModel(line, prefix);
            }
        }
        public void Dispose()
        {
            NmeaSentenseParseService = null;
            CurrentModel = null!;
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
