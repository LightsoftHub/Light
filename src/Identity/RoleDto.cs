namespace Light.Identity;

public record RoleDto
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<ClaimDto> Claims { get; set; } = [];
}
