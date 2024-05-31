using Light.Serilog;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OptionsController(IConfiguration configuration) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var config = SerilogOptionsExtensions.GetWriteToOptions(configuration);
            return Ok(config);
        }
    }
}
