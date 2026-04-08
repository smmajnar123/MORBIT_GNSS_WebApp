using MORBIT_GNSS_APP.Repository.IRepository;
using MORBIT_GNSS_APP.Service.Models;

namespace MORBIT_GNSS_APP.Core.Interface
{
    public interface ISerialGnssServiceModel
    {
        IGnssDataRepository GnssDataRepository { get; set; }
        List<GnssDataModel> GnssDataModels { get; set; }
        IGnssEvent GnssEvent { get; }
        bool IsConnected { get; set; }
        void Connect(string portName, int baudRate);
        void Disconnect();
    }
}
