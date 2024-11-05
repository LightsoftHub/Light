namespace Light.Identity;

public class CreateRoleRequest
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? TenantId { get; set; }
}
