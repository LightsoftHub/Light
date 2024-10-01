using Light.Domain.Entities;

namespace Sample.Data;

public partial class RetailCategory : AuditableEntity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<RetailProduct> RetailProducts { get; } = new List<RetailProduct>();
}
