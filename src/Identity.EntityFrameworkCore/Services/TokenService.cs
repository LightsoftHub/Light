using Light.Identity.EntityFrameworkCore;
using Light.Identity.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Light.Identity.Services;

public class TokenService(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IClaimType claimType,
    IOptions<JwtOptions> jwtOptions,
    IIdentityContext context) : ITokenService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    /// <summary>
    /// Get all claims of User
    /// </summary>
    public virtual async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
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

    private async Task<string> GenerateJwtAsync(User user)
    {
        var secret = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);

        var claims = await GetClaimsAsync(user);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            claims: claims,
            expires: DateTime.Now.AddSeconds(_jwtOptions.ExpiresIn),
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();

        var encryptedToken = tokenHandler.WriteToken(token);

        return encryptedToken;
    }

    private async Task<TokenDto> IssueTokenForUserAsync(User user)
    {
        var token = await GenerateJwtAsync(user);
        var tokenExpiryTime = _jwtOptions.ExpiresIn;
        var refreshToken = JwtHelper.GenerateRefreshToken();
        var refreshTokenExpiryTime = _jwtOptions.RefreshTokenExpiresIn;

        // update user login info
        await SaveTokenAsync(user.Id, refreshToken, DateTime.Now.AddSeconds(refreshTokenExpiryTime));

        return new TokenDto
        {
            AccessToken = token,
            ExpiresIn = tokenExpiryTime,
            RefreshToken = refreshToken,
            RefreshTokenExpiresIn = refreshTokenExpiryTime,
        };
    }

    /// <summary>
    /// Save token info to DB
    /// </summary>
    private async Task SaveTokenAsync(string userId, string refreshToken, DateTime refreshTokenExpiryTime)
    {
        var entry = await context.JwtTokens.FindAsync(userId);

        if (entry is not null)
        {
            entry.RefreshToken = refreshToken;
            entry.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await context.SaveChangesAsync();
        }
        else
        {
            var entity = new JwtToken
            {
                UserId = userId,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshTokenExpiryTime,
            };

            await context.JwtTokens.AddAsync(entity);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Check refresh token is valid & not expired
    /// </summary>
    private async Task<bool> CheckRefreshTokenAsync(string userId, string refreshToken)
    {
        return await context.JwtTokens.AnyAsync(x =>
            x.UserId == userId
            && x.RefreshToken == refreshToken
            && x.RefreshTokenExpiryTime >= DateTime.Now);
    }

    public virtual async Task<IResult<TokenDto>> GetTokenAsync(User? user)
    {
        if (user == null
            || user.Status.IsActive is false
            || user.IsDeleted)
            return Result<TokenDto>.Error("Invalid credentials.");

        var token = await IssueTokenForUserAsync(user);

        return Result<TokenDto>.Success(token);
    }

    public async Task<IResult<TokenDto>> GetTokenByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        return await GetTokenAsync(user);
    }

    public async Task<IResult<TokenDto>> GetTokenByUserNameAsync(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);

        return await GetTokenAsync(user);
    }

    public virtual async Task<IResult<TokenDto>> RefreshTokenAsync(string accessToken, string refreshToken)
    {
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
        var isTokenValid = await CheckRefreshTokenAsync(userId, refreshToken);

        if (isTokenValid is false)
            return Result<TokenDto>.Unauthorized("Invalid refresh token.");

        var user = await userManager.FindByIdAsync(userId);

        if (user == null || user.Status.IsActive is false || user.IsDeleted)
            return Result<TokenDto>.Unauthorized("User not found or inactive.");

        var token = await IssueTokenForUserAsync(user);

        return Result<TokenDto>.Success(token);
    }
}
