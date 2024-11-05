namespace Light.Identity.Options;

public class JwtOptions
{
    public string Issuer { get; set; } = "https://localhost";

    public string SecretKey { get; set; } = "Qwerty!@#$%^123456"; // must length > 18

    public int ExpiresIn { get; set; } = 86400; // 1 days

    public int RefreshTokenExpiresIn { get; set; } = 604800; //7 days
}