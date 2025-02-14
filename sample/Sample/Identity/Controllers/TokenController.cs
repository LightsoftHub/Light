using Light.ActiveDirectory.Interfaces;
using Light.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Identity.Controllers;

[AllowAnonymous]
[Route("api/oauth")]
public class TokenController(ITokenService tokenService) : VersionedApiController
{
    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(string userName)
    {
        var token = await tokenService.GetTokenByUserNameAsync(userName);

        return Ok(token);
    }

    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var token = await tokenService.RefreshTokenAsync(accessToken, refreshToken);

        return Ok(token);
    }
}