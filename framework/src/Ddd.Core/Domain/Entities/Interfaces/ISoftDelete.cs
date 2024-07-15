namespace Light.Domain.Entities.Interfaces;

public interface ISoftDelete
{
    bool IsDeleted { get; }

    DateTimeOffset? DeletedOn { get; }

    string? DeletedBy { get; }
}