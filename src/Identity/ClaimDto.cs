namespace Light.Identity;

public record ClaimDto
{
    public string Type { get; set; } = null!;

    public string Value { get; set; } = null!;
}
