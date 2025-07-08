using Light.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Mediator;

[Route("[controller]")]
[ApiController]
public class LightMediatorController(ISender sender) : ControllerBase
{
    [HttpGet("value")]
    public async Task<IActionResult> GetAsync()
    {
        var res = await sender.Send(new GetValue.Query());
        return Ok(res);
    }
}
