namespace Light.Identity;

public record UserDto
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public bool UseDomainPassword { get; set; }

    public IdentityStatus Status { get; set; }

    public bool IsDeleted { get; set; }

    public IEnumerable<string> Roles { get; set; } = [];

    public IEnumerable<ClaimDto> Claims { get; set; } = [];
}