using Light.Identity;
using Light.Identity.EntityFrameworkCore.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        return Ok(await userService.GetPagedAsync(request));
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

    [HttpGet("{userId}/get_attributes")]
    public async Task<IActionResult> GetAttributes(string userId)
    {
        var obj = await userAttributeService.GetUserAttributesAsync(userId);

        return Ok(obj);
    }

    [HttpPut("{userId}/update_attribute")]
    public async Task<IActionResult> GetAttributes(string userId, Dictionary<string, string> values)
    {
        foreach (var record in values)
        {
            await userAttributeService.AddAsync(userId, record.Key, record.Value);
        }

        return Ok();
    }

    [HttpDelete("attributes")]
    public async Task<IActionResult> DeleteAttribute(string userId, string key)
    {
        var obj = await userAttributeService.DeleteAsync(userId, key);

        return Ok(obj);
    }
}
