using MORBIT_GNSS_APP.Shared.Common;
using System.IO.Ports;

namespace MORBIT_GNSS_APP.Shared.Helpers
{
    public class SerialGnssHelper
    {
        public static string[] GetPorts()
        {
            try
            {
                var ports = SerialPort.GetPortNames();
                if (ports.Length == 0)
                    Logger.Write("No serial ports found.");

                return ports;
            }
            catch (Exception ex)
            {
                Logger.Write($"Error retrieving serial ports: {ex.Message}");
                return [];
            }
        }

        public static string[] GetBaudRates()
        {
            return ["9600", "19200", "38400", "57600", "115200"];
        }
    }
}
