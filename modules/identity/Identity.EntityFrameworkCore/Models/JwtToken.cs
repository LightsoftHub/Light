namespace Light.Identity.EntityFrameworkCore.Models;

public class JwtToken
{
    public string UserId { get; set; } = null!;

    public string? Token { get; set; }

    public DateTime? TokenExpiryTime { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}
