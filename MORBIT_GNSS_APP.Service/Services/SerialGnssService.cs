using Microsoft.Extensions.Logging;
using MORBIT_GNSS_APP.Service.IService;
using MORBIT_GNSS_APP.Shared.Common;
using MORBIT_GNSS_APP.Shared.Constants;
using System.IO.Ports;

namespace MORBIT_GNSS_APP.Service.Services
{
    public class SerialGnssService : ISerialGnssService
    {
        #region Fields
        private SerialPort? _port;
        private readonly DeviceLogBuffer _deviceLogBuffer;
        private readonly CancellationTokenSource? _cts = new();
        public bool IsConnected => _port != null && _port.IsOpen;
        ILogger<SerialGnssService> _logger;
        #endregion

#if DEBUG
        public List<string> testingLogData;
        public int testingIndex = 0;
        #endif

        #region Constructor
        public SerialGnssService(ILogger<SerialGnssService> logger)
        {
            _logger = logger;
            _deviceLogBuffer = new DeviceLogBuffer();
            StartWorker(_cts.Token);
            #if DEBUG
            try
            {
                testingLogData = [.. File.ReadAllLines(@"D:\M0201_MorbitGnns_2502261637.txt").Where(static x => !string.IsNullOrWhiteSpace(x) && x.StartsWith('$'))];
                for (int i = 0; i < testingLogData.Count; i++)
                {
                    DeviceLogBuffer.DeviceLogQueue.Enqueue(testingLogData[i]);
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"Error loading test data: {ex.Message}");
                testingLogData = [];
            }
            #endif
        }
        #endregion

        #region properties
        public DeviceLogBuffer DeviceLogBuffer { get { return _deviceLogBuffer; } }

        public event Action<string>? OnLineReceived;
        #endregion

        #region Methods
        public void Connect(string portName, int baudRate)
        {
            Logger.Write($"Connecting to port {portName} at {baudRate} baud.");
            if (IsConnected)
            {
                Disconnect();
            }
            _port = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
            {
                NewLine = "\r\n"
            };
            _port.DataReceived += DataReceived;
            _port.Open();
            Logger.Write("Serial port connected.");
            _logger.LogInformation($"Connected to serial port {portName} at {baudRate} baud.");
        }
        public void Disconnect()
        {
            if (_port == null) return;
            _port.DataReceived -= DataReceived;
            if (_port.IsOpen)
            {
                _port.Close();
            }
            _port.Dispose();
            _port = null;
            Logger.Write("Serial disconnected");
            _logger.LogInformation("Disconnected from serial port.");
        }
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (_port == null || !_port.IsOpen)
                    return;
                string line = _port.ReadLine();
                #if DEBUG
                if (testingIndex < testingLogData.Count)
                {
                    line = testingLogData[testingIndex++];
                }
                #endif
                if (!string.IsNullOrWhiteSpace(line))
                    DeviceLogBuffer.Add(line);

            }
            catch (Exception ex)
            {
                Logger.Write($"Serial error: {ex.Message}");
            }
        }
        private async void StartWorker(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                int batch = 0;
                while (DeviceLogBuffer.DeviceLogQueue.TryDequeue(out var line) && batch < Constant.MAX_LOG)
                {
                    OnLineReceived?.Invoke(line);
                    batch++;
                }
                await Task.Delay(50,token);
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Disconnect();
            _cts?.Cancel();
            _cts?.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}