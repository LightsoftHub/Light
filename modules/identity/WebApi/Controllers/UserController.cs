using Light.ActiveDirectory.Interfaces;
using Light.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserController(
    IUserService userService,
    IUserAttributeService userAttributeService,
    IActiveDirectoryService activeDirectoryService) : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var res = await userService.GetAllAsync(cancellationToken);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        var res = await userService.GetByIdAsync(id);
        return Ok(res);
    }

    [HttpGet("by_user_name/{userName}")]
    public async Task<IActionResult> GetByUserNameAsync([FromRoute] string userName)
    {
        var res = await userService.GetByUserNameAsync(userName);
        return Ok(res);
    }

    [HttpGet("{id}/attribute")]
    public async Task<IActionResult> GetAttributeAsync([FromRoute] string id)
    {
        var res = await userAttributeService.GetUserAttributesAsync(id);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateUserRequest request)
    {
        var res = await userService.CreateAsync(request);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync([FromBody] UserDto request)
    {
        var res = await userService.UpdateAsync(request);
        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        return Ok(await userService.DeleteAsync(id));
    }

    [HttpPut("force_password")]
    public async Task<IActionResult> ForcePasswordAsync([FromBody] ForcePasswordRequest request)
    {
        var res = await userService.ForcePasswordAsync(request.UserId, request.Password);
        return Ok(res);
    }

    [HttpGet("get_domain_user/{userName}")]
    public async Task<IActionResult> GetDomainUserAsync([FromRoute] string userName)
    {
        var res = await activeDirectoryService.GetByUserNameAsync(userName);
        return Ok(res);
    }
}