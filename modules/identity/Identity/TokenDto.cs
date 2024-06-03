namespace Light.Identity;

public class TokenDto
{
    public string AccessToken { get; set; } = null!;

    public int ExpiresIn { get; set; }

    public string RefreshToken { get; set; } = null!;

    public int RefreshTokenExpiresIn { get; set; }
}