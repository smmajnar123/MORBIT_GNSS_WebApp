using MORBIT_GNSS_APP.Service.IService;

namespace MORBIT_GNSS_APP.Core.Interface
{
    public interface INmeaLogParseServiceModel : IDisposable
    {
        INmeaBaseModel CurrentModel { get; set; }
        INmeaEvent NmeaEvent { get; }
        void Send(string line);
    }
}
