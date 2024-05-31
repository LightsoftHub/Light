using Light.Domain.Entities.Interfaces;

namespace Sample.Data;

public partial class RetailCategory : IEntity
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual ICollection<RetailProduct> RetailProducts { get; } = new List<RetailProduct>();
}
