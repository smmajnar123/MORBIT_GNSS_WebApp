using Microsoft.AspNetCore.Mvc;
using MORBIT_GNSS_APP.Shared.Helpers;

namespace MORBIT_GNSS_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialController : ControllerBase
    {
        // ✅ GET: api/serial/ports
        [HttpGet("ports")]
        public IActionResult GetPorts()
        {
            var ports = SerialGnssHelper.GetPorts();

            if (ports == null || ports.Length == 0)
                return NotFound("No COM ports found");

            return Ok(ports);
        }

        // ✅ GET: api/serial/baudrates
        [HttpGet("baudrates")]
        public IActionResult GetBaudRates()
        {
            var baudRates = SerialGnssHelper.GetBaudRates();
            return Ok(baudRates);
        }
    }
}
