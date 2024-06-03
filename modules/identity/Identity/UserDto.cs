namespace Light.Identity;

public class UserDto
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public bool UseDomainPassword { get; set; }

    public IdentityStatus Status { get; set; }

    public bool IsDeleted { get; set; }

    public IEnumerable<string> Roles { get; set; } = new List<string>();
}