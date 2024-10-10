namespace Light.Domain.Entities;

/// <summary>
///     A base class for DDD Auditable Entities. Includes support for domain events dispatched post-persistence.
///     use string for type of ID and set default is NewGuid as string.
/// </summary>
public abstract class AuditableEntity : BaseAuditableEntity<string>
{
    protected AuditableEntity() => Id = Ulid.NewUlid().ToString();
}
