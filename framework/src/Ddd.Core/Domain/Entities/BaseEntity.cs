using Light.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Light.Domain.Entities;

/// <summary>
///     A base class for DDD Entities. Includes support for domain events dispatched post-persistence.
/// </summary>
public abstract class BaseEntity : IEntity, IEvent
{
    private readonly List<DomainEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

/// <summary>
///     A base class for DDD Entities. Includes support for domain events dispatched post-persistence.
///     support both GUID and int IDs, change to EntityBase&lt;TId&gt; and use TId as the type for Id.
/// </summary>
public abstract class BaseEntity<TId> : BaseEntity, IEntity<TId>
{
    public virtual TId Id { get; protected set; } = default!;
}