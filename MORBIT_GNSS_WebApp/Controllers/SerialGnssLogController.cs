using Microsoft.AspNetCore.Mvc;
using MORBIT_GNSS_APP.Core.Interface;
using MORBIT_GNSS_APP.Shared.Helpers;
using MORBIT_GNSS_WebApp.RequestDto;

namespace MORBIT_GNSS_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialGnssLogController : ControllerBase
    {
        private readonly ISerialGnssServiceModel serialGnssServiceModel;
        private readonly ILogger<SerialGnssLogController> _logger;

        public SerialGnssLogController(ILogger<SerialGnssLogController> logger, ISerialGnssServiceModel serialGnssServiceModel)
        {
            _logger = logger;
            this.serialGnssServiceModel = serialGnssServiceModel;
            serialGnssServiceModel.GnssEvent.OnLogReceived += Gnss_OnLineReceived;
        }

        private void Gnss_OnLineReceived(string obj)
        {
            _logger.LogInformation($"Received GNSS Log: {obj}");
            GnssLogStore.Add(obj);
        }
       
        // ✅ CONNECT / DISCONNECT
        [HttpPost("connect")]
        public IActionResult Connect([FromBody] SerialConnectRequestDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Port))
                    return BadRequest("Port is required");

                if (request.BaudRate <= 0)
                    return BadRequest("Invalid baud rate");

                if (!serialGnssServiceModel.IsConnected)
                {
                    serialGnssServiceModel.Connect(request.Port, request.BaudRate);
                    if (serialGnssServiceModel.IsConnected)
                    {
                        return Ok(new
                        {
                            Status = serialGnssServiceModel.IsConnected,
                            request.Port,
                            request.BaudRate
                        });
                    }
                    else
                    {
                         return StatusCode(500, "Failed to connect to the GNSS device");
                    }
                }
                else
                {
                    serialGnssServiceModel.Disconnect();
                    return Ok(new
                    {
                        Status = "Disconnected"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // ✅ OPTIONAL: Separate Disconnect API
        [HttpPost("disconnect")]
        public IActionResult Disconnect()
        {
            try
            {
                if (!serialGnssServiceModel.IsConnected)
                    return BadRequest("Already disconnected");
                serialGnssServiceModel.Disconnect();
                return Ok("Disconnected successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ STATUS API
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new
            {
                serialGnssServiceModel.IsConnected
            });
        }

        [HttpGet("logs")]
        public IActionResult GetLogs()
        {
            List<string> logs = [];
            _logger.LogInformation("Fetching GNSS logs" + GnssLogStore.Get().Count);
            if (serialGnssServiceModel.IsConnected)
            {
                logs = GnssLogStore.Get();
            }
            else
            {
                return BadRequest("Not connected to any GNSS device");
            }
            return Ok((List<string>)[]);
        }
    }
}
