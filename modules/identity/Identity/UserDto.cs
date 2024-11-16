﻿namespace Light.Identity;

public class UserDto
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

    public string? TenantId { get; set; }

    public IEnumerable<string> Roles { get; set; } = new List<string>();
}