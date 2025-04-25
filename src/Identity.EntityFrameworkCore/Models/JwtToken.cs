using Light.Domain.Entities;

namespace Light.Identity.Models;

public class JwtToken : BaseEntity<string>
{
    public JwtToken() => Id = LightId.NewId();

    public string UserId { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTimeOffset TokenExpiresAt { get; set; }

    public string? RefreshToken { get; set; }

    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }

    public bool Revoked { get; set; }

    public string? DeviceId { get; set; }

    public string? DeviceName { get; set; }
}
