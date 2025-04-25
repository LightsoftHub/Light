using Light.Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Light.Identity.EntityFrameworkCore;

public class TokenService(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IClaimType claimType,
    IOptions<JwtOptions> jwtOptions,
    IIdentityContext context) : ITokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    private static DateTimeOffset Now => DateTimeOffset.UtcNow;

    public async Task<IResult<TokenDto>> GenerateTokenByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        return await GetTokenAsync(user);
    }

    public async Task<IResult<TokenDto>> GenerateTokenByUserNameAsync(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);

        return await GetTokenAsync(user);
    }

    public virtual async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var userRoles = await userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();

        foreach (var userRole in userRoles)
        {
            roleClaims.Add(new Claim(claimType.Role, userRole));

            var role = await roleManager.FindByNameAsync(userRole);
            if (role is null)
                continue;

            var allClaimsForThisRoles = await roleManager.GetClaimsAsync(role);

            permissionClaims.AddRange(allClaimsForThisRoles);
        }

        var claims = new List<Claim>
        {
            new(claimType.UserId, user.Id ?? string.Empty),
            new(claimType.UserName, user.UserName ?? string.Empty),
            new(claimType.FirstName, user.FirstName ?? string.Empty),
            new(claimType.LastName, user.LastName ?? string.Empty),
            new(claimType.PhoneNumber, user.PhoneNumber ?? string.Empty),
            new(claimType.Email, user.Email ?? string.Empty),
        }
        .Union(userClaims)
        .Union(roleClaims)
        .Union(permissionClaims);

        return claims;
    }

    private async Task<TokenDto> GenerateTokenAsync(User user)
    {
        var secret = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);

        var claims = await GetUserClaimsAsync(user);

        var expiresInSeconds = _jwtOptions.AccessTokenExpirationSeconds;
        var expiresOn = DateTime.Now.AddSeconds(expiresInSeconds);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            claims: claims,
            expires: expiresOn,
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();

        var encryptedToken = tokenHandler.WriteToken(token);
        var refreshToken = JwtHelper.GenerateRefreshToken();

        return new TokenDto(encryptedToken, expiresInSeconds, refreshToken);
    }

    public virtual async Task<IResult<TokenDto>> GetTokenAsync(User? user)
    {
        var now = Now;

        if (user == null
            || user.Status.IsActive is false
            || user.Deleted == null)
            return Result<TokenDto>.Error("Invalid credentials.");

        var token = await GenerateTokenAsync(user);

        var tokenExpiresAt = now.AddSeconds(token.ExpiresIn);
        var refreshTokenExpiresAt = now.AddDays(_jwtOptions.RefreshTokenExpirationDays);

        var entity = new JwtToken
        {
            UserId = user.Id,
            Token = token.AccessToken,
            TokenExpiresAt = tokenExpiresAt,
            RefreshToken = token.RefreshToken,
            RefreshTokenExpiresAt = refreshTokenExpiresAt,
        };

        await context.JwtTokens.AddAsync(entity);
        await context.SaveChangesAsync();

        return Result<TokenDto>.Success(token);
    }

    public virtual async Task<IResult<TokenDto>> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var now = Now;

        // get UserPrincipal from expired token
        var userPrincipal = JwtHelper.GetPrincipalFromExpiredToken(
            accessToken,
            _jwtOptions.SecretKey,
            _jwtOptions.Issuer,
            claimType.Role);

        // get userID from UserPrincipal
        var userId = userPrincipal.FindFirstValue(claimType.UserId);

        if (string.IsNullOrEmpty(userId))
            return Result<TokenDto>.Unauthorized("Error when read info from token.");

        // check refresh token is exist and not out of lifetime
        var userToken = await GetValidTokenAsync(userId, refreshToken);

        if (userToken is null)
            return Result<TokenDto>.Unauthorized("Invalid refresh token.");

        var user = await userManager.FindByIdAsync(userId);

        if (user == null || user.Status.IsActive is false || user.Deleted == null)
            return Result<TokenDto>.Unauthorized("User not found or inactive.");

        var token = await GenerateTokenAsync(user);

        var tokenExpiresAt = now.AddSeconds(token.ExpiresIn);
        var refreshTokenExpiresAt = now.AddDays(_jwtOptions.RefreshTokenExpirationDays);

        // save token data
        userToken.Token = token.AccessToken;
        userToken.TokenExpiresAt = tokenExpiresAt;
        userToken.RefreshToken = token.RefreshToken;
        userToken.RefreshTokenExpiresAt = refreshTokenExpiresAt;

        await context.SaveChangesAsync();

        return Result<TokenDto>.Success(token);
    }

    private Task<JwtToken?> GetValidTokenAsync(string userId, string refreshToken)
    {
        var now = Now;

        return context.JwtTokens
            .Where(x =>
                x.UserId == userId
                && x.RefreshToken == refreshToken
                && x.RefreshTokenExpiresAt >= now
                && x.Revoked == false)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<UserTokenDto>> GetUserTokensAsync(string userId)
    {
        var now = DateTimeOffset.UtcNow;

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
