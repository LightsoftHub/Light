﻿namespace Light.Identity;

public class RoleDto
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<ClaimDto> Claims { get; set; } = [];
}
