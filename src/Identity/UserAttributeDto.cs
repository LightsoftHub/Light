namespace Light.Identity;

public record UserAttributeDto
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;
}
