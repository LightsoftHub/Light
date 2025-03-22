namespace Light.Identity;

public interface ITokenService
{
    /// <summary>
    /// Generate access token by UserID
    /// </summary>
    Task<IResult<TokenDto>> GenerateTokenByIdAsync(string userId);

    /// <summary>
    /// Generate access token by UserName
    /// </summary>
    Task<IResult<TokenDto>> GenerateTokenByUserNameAsync(string userName);

    /// <summary>
    /// Generate access token from old token and refresh token
    /// </summary>
    Task<IResult<TokenDto>> RefreshTokenAsync(string accessToken, string refreshToken);

    /// <summary>
    /// Get user tokens list
    /// </summary>
    Task<IEnumerable<UserTokenDto>> GetUserTokensAsync(string userId);

    /// <summary>
    /// Revoke token
    /// </summary>
    Task RevokedAsync(string userId, string tokenId);
}