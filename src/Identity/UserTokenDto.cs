namespace Light.Identity;

public record UserTokenDto
{
    public string Id { get; set; } = null!;

    public DateTimeOffset? ExpiresAt { get; set; }

    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }

    public string? DeviceId { get; set; }

    public string? DeviceName { get; set; }
}