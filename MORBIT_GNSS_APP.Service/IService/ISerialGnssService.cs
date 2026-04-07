namespace MORBIT_GNSS_APP.Service.IService
{
    public interface ISerialGnssService : IDisposable
    {
        event Action<string>? OnLineReceived;
        bool IsConnected { get;}
        void Connect(string portName, int baudRate); 
        void Disconnect(); 
    }
}
