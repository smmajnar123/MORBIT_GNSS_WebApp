namespace MORBIT_GNSS_APP.Core.Interface
{
    public interface ISerialGnssServiceModel
    {
        IGnssEvent GnssEvent { get; }
        bool IsConnected { get; set; }
        void Connect(string portName, int baudRate);
        void Disconnect();
    }
}
