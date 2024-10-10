using Light.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Light.Identity.EntityFrameworkCore.Models;

public class Role : IdentityRole, IEntity, IAuditableEntity
{
    public Role() => Id = LightId.NewId();

    public string? Description { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public void Update(string? name, string? description)
    {
        Name = name;
        Description = description;
    }
}