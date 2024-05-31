namespace Light.Domain.Entities.Default;

/// <summary>
///     A base class for DDD Entities with default ID using MassTransit.NewId
/// </summary>
public abstract class BaseEntity : BaseEntity<string>
{
    protected BaseEntity() => Id = Guid.NewGuid().ToString();
}

/// <summary>
///     A base class for DDD Auditable Entities with default ID using MassTransit.NewId
/// </summary>
public abstract class AuditableEntity : AuditableEntity<string>
{
    protected AuditableEntity() => Id = Guid.NewGuid().ToString();
}