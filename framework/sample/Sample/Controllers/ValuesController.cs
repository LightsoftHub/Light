using Light.Contracts;
using Light.Extensions;
using Light.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var value = "";
            return Ok(value.Left("-"));
        }

        [HttpGet("right")]
        public IActionResult Get(string data, int length)
        {
            return Ok(data.Right(length));
        }

        [HttpGet("null-checker")]
        public IActionResult NullCheck()
        {
            Result? result = default;

            //result.ThrowIfNull();

            if (result is null)
                throw new NotFoundException("Result not found");

            return Ok();
        }
    }
}
