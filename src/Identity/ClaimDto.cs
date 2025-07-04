namespace Light.Identity;

public record ClaimDto
{
    public required string Type { get; set; }

    public required string Value { get; set; }
}
