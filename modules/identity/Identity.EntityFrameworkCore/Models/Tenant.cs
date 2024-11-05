using Light.Domain.Entities;
using Light.Domain.Entities.Interfaces;

namespace Light.Identity.Models;

public class Tenant : AuditableEntity, ISoftDelete
{
    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }
}
