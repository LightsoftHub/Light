using Light.Identity;
using Light.Identity.EntityFrameworkCore.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(
    IUserService userService,
    IUserAttributeService userAttributeService,
    IOptions<ClaimTypeOptions> claims) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await userService.GetAllAsync());
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetAsync([FromQuery] SearchUserRequest request)
    {
        return Ok(await userService.GetAllAsync());
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(CreateUserRequest request)
    {
        return Ok(await userService.CreateAsync(request));
    }

    [HttpGet("claims")]
    public IActionResult GetClaims()
    {
        var obj = claims.Value;

        var value = new
        {
            claim = obj,
            user = CustomClaims.UserName
        };

        return Ok(value);
    }

    [HttpGet("attributes/{id}")]
    public async Task<IActionResult> GetAttributes(string id)
    {
        var obj = await userAttributeService.GetUserAttributesAsync(id);

        return Ok(obj);
    }

    [HttpGet("attributes")]
    public async Task<IActionResult> GetAttributes(string userId, string key, string value)
    {
        var obj = await userAttributeService.AddAsync(userId, key, value);

        return Ok(obj);
    }

    [HttpDelete("attributes")]
    public async Task<IActionResult> DeleteAttribute(string userId, string key)
    {
        var obj = await userAttributeService.DeleteAsync(userId, key);

        return Ok(obj);
    }
}
