using Azure.Core;
using Light.Identity;
using Light.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Identity.Controllers;

public class TokenController(JwtTokenMananger tokenService) : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await Task.Yield(); // Simulate async operation

        return Ok(tokenService.TimeNow);
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(string userName)
    {
        var user = await tokenService.UserManager.FindByNameAsync(userName);

        return Ok(await tokenService.GenerateTokenByAsync(user!, "Web", "Chrome"));
    }

    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshTokenAsync(string userName, string refreshToken)
    {
        var user = await tokenService.UserManager.FindByNameAsync(userName);

        return Ok(await tokenService.RefreshTokenAsync(user!, refreshToken));
    }

    [HttpPost("token/revoke")]
    public async Task<IActionResult> RevokeAsync(string userId, string tokenId)
    {
        await tokenService.RevokedAsync(userId, tokenId);
        return Ok();
    }

    [HttpGet("token/list/{userId}")]
    public async Task<IActionResult> GetListAsync(string userId)
    {
        var res = await tokenService.GetUserTokensAsync(userId);
        return Ok(res);
    }
}