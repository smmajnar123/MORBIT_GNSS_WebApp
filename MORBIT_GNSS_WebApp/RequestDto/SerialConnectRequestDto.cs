namespace MORBIT_GNSS_WebApp.RequestDto
{
    public class SerialConnectRequestDto
    {
        public string Port { get; set; } = string.Empty;
        public int BaudRate { get; set; }
    }
}
