using MORBIT_GNSS_APP.Shared.Constants;
using System.Collections.Concurrent;

namespace MORBIT_GNSS_APP.Shared.Common
{
    public class DeviceLogBuffer
    {
        public ConcurrentQueue<string> DeviceLogQueue { get; } = new();
        public int MaxSize { get; set; } = Constant.MAX_LOG;
        public void Add(string line)
        {
            if (DeviceLogQueue.Count < MaxSize)
                DeviceLogQueue.Enqueue(line);
        }
    }
}
