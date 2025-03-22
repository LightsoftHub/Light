namespace Light.Identity;

public class UserTokenDto
{
    public string Id { get; set; } = null!;

    public DateTimeOffset? ExpireOn { get; set; }

    public DateTimeOffset? RefreshTokenExpireOn { get; set; }

    public string? DeviceId { get; set; }

    public string? DeviceName { get; set; }
}