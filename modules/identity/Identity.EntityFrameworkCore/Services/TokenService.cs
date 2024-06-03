using Light.Identity.EntityFrameworkCore.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Light.Identity.EntityFrameworkCore.Services;

public class TokenService(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IOptions<ClaimTypeOptions> claimTypes,
    IOptions<JwtOptions> jwtOptions,
    IIdentityDbContext db) : ITokenService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly ClaimTypeOptions _claimTypes = claimTypes.Value;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly IIdentityDbContext _db = db;

    /// <summary>
    /// Get all claims of User
    /// </summary>
    private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();

        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(_claimTypes.Role, role));

            var thisRole = await _roleManager.FindByNameAsync(role);
            if (thisRole is null)
                continue;

            var allClaimsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);

            permissionClaims.AddRange(allClaimsForThisRoles);
        }

        var claims = new List<Claim>
        {
            new(_claimTypes.UserId, user.Id ?? string.Empty),
            new(_claimTypes.UserName, user.UserName ?? string.Empty),
            new(_claimTypes.FirstName, user.FirstName ?? string.Empty),
            new(_claimTypes.LastName, user.LastName ?? string.Empty),
            new(_claimTypes.PhoneNumber, user.PhoneNumber ?? string.Empty),
            new(_claimTypes.Email, user.Email ?? string.Empty),
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
        var entry = await _db.JwtTokens.FindAsync(userId);

        if (entry is not null)
        {
            entry.RefreshToken = refreshToken;
            entry.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _db.SaveChangesAsync();
        }
        else
        {
            var entity = new JwtToken
            {
                UserId = userId,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshTokenExpiryTime,
            };

            await _db.JwtTokens.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Check refresh token is valid & not expired
    /// </summary>
    private async Task<bool> CheckRefreshTokenAsync(string userId, string refreshToken)
    {
        return await _db.JwtTokens.AnyAsync(x =>
            x.UserId == userId
            && x.RefreshToken == refreshToken
            && x.RefreshTokenExpiryTime >= DateTime.Now);
    }

    public async Task<IResult<TokenDto>> GetTokenAsync(User? user)
    {
        if (user == null
            || user.Status.IsActive is false
            || user.IsDeleted)
            return Result<TokenDto>.BadRequest("Invalid credentials.");

        var token = await IssueTokenForUserAsync(user);

        return Result<TokenDto>.Success(token);
    }

    public async Task<IResult<TokenDto>> GetTokenByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return await GetTokenAsync(user);
    }

    public async Task<IResult<TokenDto>> GetTokenByUserNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        return await GetTokenAsync(user);
    }

    public async Task<IResult<TokenDto>> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        // get UserPrincipal from expired token
        var userPrincipal = JwtHelper.GetPrincipalFromExpiredToken(
            accessToken,
            _jwtOptions.SecretKey,
            _jwtOptions.Issuer);

        // get userID from UserPrincipal
        var userId = userPrincipal.FindFirstValue(_claimTypes.UserId);
        if (string.IsNullOrEmpty(userId))
            return Result<TokenDto>.BadRequest("Invalid token.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.Status.IsActive is false)
            return Result<TokenDto>.BadRequest("Invalid token.");

        // check refresh token is exist or not out of lifetime
        var isTokenValid = await CheckRefreshTokenAsync(userId, refreshToken);
        if (isTokenValid is false)
            return Result<TokenDto>.BadRequest("Invalid token.");

        var token = await IssueTokenForUserAsync(user);

        return Result<TokenDto>.Success(token);
    }
}
