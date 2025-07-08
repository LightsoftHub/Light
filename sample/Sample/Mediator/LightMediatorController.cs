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

    [HttpPut("value")]
    public async Task<IActionResult> PutAsync(string id)
    {
        var res = await sender.Send(new UpdateValue.Command(id));
        return Ok(res);
    }

    [HttpDelete("value")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await sender.Send(new DeleteValue.Command(id));
        return Ok();
    }
}
