namespace Light.Identity;

public interface ITokenService
{
    /// <summary>
    /// Generate access token by UserID
    /// </summary>
    Task<IResult<TokenDto>> GetTokenByIdAsync(string userId);

    /// <summary>
    /// Generate access token by UserName
    /// </summary>
    Task<IResult<TokenDto>> GetTokenByUserNameAsync(string userName);

    /// <summary>
    /// Generate access token from old token and refresh token
    /// </summary>
    Task<IResult<TokenDto>> RefreshTokenAsync(string accessToken, string refreshToken);
}