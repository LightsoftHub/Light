using Light.Domain.Entities;

#nullable disable

namespace Sample.Data;

public class RetailCategory : AuditableEntity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<RetailProduct> RetailProducts { get; } = new List<RetailProduct>();
}
