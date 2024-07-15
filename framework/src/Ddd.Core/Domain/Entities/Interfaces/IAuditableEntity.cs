namespace Light.Domain.Entities.Interfaces;

public interface IAuditableEntity
{
    DateTimeOffset CreatedOn { get; }

    string? CreatedBy { get; }

    DateTimeOffset? LastModifiedOn { get; }

    string? LastModifiedBy { get; }
}