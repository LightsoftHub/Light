using Light.Domain.Entities;

namespace Sample.Data;

public partial class RetailProduct : AuditableEntity
{
    public string CategoryId { get; set; } = null!;

    public string StoreCode { get; set; } = null!;

    public string? Sku { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Unit { get; set; } = null!;

    public double UnitPrice { get; set; }

    public string? MainImageUrl { get; set; }

    public int Status { get; set; }

    public virtual RetailCategory Category { get; set; } = null!;
}
