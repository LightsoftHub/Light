using Light.Domain.Entities.Interfaces;

namespace Sample.Data;

public partial class RetailLocation : IEntity
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual ICollection<RetailStore> RetailStores { get; } = new List<RetailStore>();
}
