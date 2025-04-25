using Light.Domain.Entities;
using Light.Domain.Entities.Interfaces;

namespace IntegrationTests;

public class Product : AuditableEntity, ISoftDelete, ITenant
{
    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTimeOffset? Deleted { get; set; }

    public string? DeletedBy { get; set; }

    public string? TenantId { get; set; }
}
