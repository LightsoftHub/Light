using Light.ActiveDirectory.Interfaces;
using Light.Identity;
using Light.Identity.Extensions;
using Light.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Sample.Identity.Controllers;

public class UserController(
    IUserService userService,
    IUserAttributeService userAttributeService,
    IActiveDirectoryService activeDirectoryService,
    UserManager<User> userManager) : VersionedApiController
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
        var res = await userAttributeService.GetByAsync(id);
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
    public async Task<IActionResult> ForcePasswordAsync(string userId, string password)
    {
        var res = await userService.ForcePasswordAsync(userId, password);
        return Ok(res);
    }

    [HttpGet("get_domain_user/{userName}")]
    public async Task<IActionResult> GetDomainUserAsync([FromRoute] string userName)
    {
        var res = await activeDirectoryService.GetByUserNameAsync(userName);
        return Ok(res);
    }

    [HttpGet("attribute/{key}/{value}")]
    public async Task<IActionResult> GetDomainUserAsync(string key, string value)
    {
        var res = await userAttributeService.GetUsersAsync(key, value);
        return Ok(res);
    }

    [HttpGet("by_claim/{key}/{value}")]
    public async Task<IActionResult> GetByClaimAsync(string key, string value)
    {
        var res = await userService.GetUsersHasClaim(key, value);
        return Ok(res);
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var user = await userManager.FindByNameAsync("super");

        await userManager.AddClaimsAsync(user, new List<Claim>
        {
            new Claim("Ckey1", "Cvalue1"),
            new Claim("Ckey2", "old2"),
            new Claim("Ckey3", "old3")
        });

        var existingClaims = await userManager.GetClaimsAsync(user);

        var requestClaims = new List<Claim>
        {
            new Claim("Ckey1", "Cvalue1"),
            new Claim("Ckey2", "new2"),
            new Claim("Ckey3", "new3")
        };

        var claimsToRemove = existingClaims.Except(requestClaims);
        await userManager.RemoveClaimsAsync(user, claimsToRemove);

        var claimsToAdd = requestClaims.Except(existingClaims);
        await userManager.AddClaimsAsync(user, claimsToAdd);

        return Ok(await userManager.GetClaimsAsync(user));
    }
}