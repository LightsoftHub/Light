using Light.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Identity.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController(ITokenService tokenService) : ControllerBase
{
    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(string userName)
    {
        var token = await tokenService.GenerateTokenByUserNameAsync(userName);

        return Ok(token);
    }

    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var token = await tokenService.RefreshTokenAsync(accessToken, refreshToken);

        return Ok(token);
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