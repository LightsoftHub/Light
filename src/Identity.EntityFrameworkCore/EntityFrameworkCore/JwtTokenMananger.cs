using Light.Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Light.Identity.EntityFrameworkCore;

public class JwtTokenMananger(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IOptions<JwtOptions> jwtOptions,
    IIdentityContext context)
{
    private readonly JwtOptions _jwt = jwtOptions.Value;

    public UserManager<User> UserManager => userManager;

    public virtual DateTimeOffset TimeNow => DateTimeOffset.UtcNow;

    public virtual async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var userRoles = await userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();

        foreach (var userRole in userRoles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var role = await roleManager.FindByNameAsync(userRole);
            if (role is null)
                continue;

            var allClaimsForThisRoles = await roleManager.GetClaimsAsync(role);

            permissionClaims.AddRange(allClaimsForThisRoles);
        }

        var claims = new List<Claim>
        {
            { ClaimTypes.UserId, user.Id },
            { ClaimTypes.UserName, user.UserName },
            { ClaimTypes.FirstName, user.FirstName },
            { ClaimTypes.LastName, user.LastName },
            { ClaimTypes.PhoneNumber, user.PhoneNumber },
            { ClaimTypes.Email, user.Email },
        }
        .Union(userClaims)
        .Union(roleClaims)
        .Union(permissionClaims)
        .Where(x => !string.IsNullOrEmpty(x.Value));

        return claims;
    }

    public virtual async Task<IResult<TokenDto>> GenerateTokenByAsync(User user, string? deviceId = null, string? deviceName = null)
    {
        var claims = await GetUserClaimsAsync(user);

        var timeNow = TimeNow.DateTime;

        var tokenExpiresInSeconds = _jwt.AccessTokenExpirationSeconds;
        var tokenExpiresAt = timeNow.AddSeconds(tokenExpiresInSeconds);

        var jwtToken = JwtHelper.GenerateToken(
            _jwt.Issuer,
            claims,
            tokenExpiresAt,
            _jwt.SecretKey);

        var refreshToken = JwtHelper.GenerateRefreshToken();
        var refreshTokenExpiresAt = timeNow.AddDays(_jwt.RefreshTokenExpirationDays);

        var newToken = new JwtToken
        {
            UserId = user.Id,
            DeviceId = deviceId,
            DeviceName = deviceName,
            Token = jwtToken,
            TokenExpiresAt = tokenExpiresAt,
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshTokenExpiresAt,
        };

        await context.JwtTokens.AddAsync(newToken);
        await context.SaveChangesAsync();

        return Result<TokenDto>.Success(new TokenDto(newToken.Token, tokenExpiresInSeconds, newToken.RefreshToken));
    }

    public virtual async Task<IResult<TokenDto>> RefreshTokenAsync(User user, string refreshToken, string roleClaimType = ClaimTypes.Role, string userIdClaimType = ClaimTypes.UserId)
    {
        // check refresh token is exist and not out of lifetime
        var userToken = await context.JwtTokens
            .Where(x =>
                x.UserId == user.Id
                && x.RefreshToken == refreshToken
                && x.RefreshTokenExpiresAt >= TimeNow
                && x.Revoked == false)
            .FirstOrDefaultAsync();

        if (userToken is null)
            return Result<TokenDto>.Unauthorized("Refresh token invalid.");

        var claims = await GetUserClaimsAsync(user);

        var timeNow = TimeNow.DateTime;

        var tokenExpiresInSeconds = _jwt.AccessTokenExpirationSeconds;
        var tokenExpiresAt = timeNow.AddSeconds(tokenExpiresInSeconds);

        var jwtToken = JwtHelper.GenerateToken(
            _jwt.Issuer,
            claims,
            tokenExpiresAt,
            _jwt.SecretKey);

        // save token data
        userToken.Token = jwtToken;
        userToken.TokenExpiresAt = tokenExpiresAt;
        userToken.RefreshToken = JwtHelper.GenerateRefreshToken();
        userToken.RefreshTokenExpiresAt = timeNow.AddDays(_jwt.RefreshTokenExpirationDays);

        await context.SaveChangesAsync();

        return Result<TokenDto>.Success(new TokenDto(userToken.Token, tokenExpiresInSeconds, userToken.RefreshToken));
    }

    public async Task<IEnumerable<UserTokenDto>> GetUserTokensAsync(string userId)
    {
        var now = TimeNow;

        var list = await context.JwtTokens
            .Where(x =>
                x.UserId == userId
                &&
                    (x.TokenExpiresAt >= now
                    || (x.RefreshTokenExpiresAt.HasValue && x.RefreshTokenExpiresAt >= now))
                && x.Revoked == false)
            .AsNoTracking()
            .Select(s => new UserTokenDto
            {
                Id = s.Id,
                ExpiresAt = s.TokenExpiresAt,
                RefreshTokenExpiresAt = s.RefreshTokenExpiresAt,
                DeviceId = s.DeviceId,
                DeviceName = s.DeviceName,
            })
            .ToListAsync();

        return list;
    }

    public Task RevokedAsync(string userId, string tokenId)
    {
        return context.JwtTokens
            .Where(x => x.Id == tokenId && x.UserId == userId)
            .ExecuteUpdateAsync(e => e.SetProperty(p => p.Revoked, true));
    }
}
