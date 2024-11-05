using Light.ActiveDirectory.Interfaces;
using Light.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[AllowAnonymous]
[Route("api/oauth")]
public class TokenController(
    IUserService userService,
    IActiveDirectoryService adService,
    ITokenService tokenService) : VersionedApiController
{
    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync([FromBody] GetTokenRequest request)
    {
        var user = await userService.GetByUserNameAsync(request.UserName);
        if (user.Succeeded is false)
            return Ok(user);

        var isDomainConfigured = adService.IsConfigured();

        if (user.Data.UseDomainPassword && isDomainConfigured)
        {
            var domainLogin = await adService.CheckPasswordSignInAsync(request.UserName, request.Password);

            if (domainLogin.Succeeded is false)
                return Ok(domainLogin);
        }
        else
        {
            var localLogin = await userService.CheckPasswordByUserNameAsync(request.UserName, request.Password);

            if (localLogin.Succeeded is false)
                return Ok(localLogin);
        }

        var token = await tokenService.GetTokenByUserNameAsync(request.UserName);

        return Ok(token);
    }

    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        var token = await tokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

        return Ok(token);
    }
}