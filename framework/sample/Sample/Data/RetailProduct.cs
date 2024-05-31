using Light.Domain.Entities.Interfaces;

namespace Sample.Data;

public partial class RetailProduct : IEntity
{
    public string Id { get; set; } = null!;

    public string CategoryId { get; set; } = null!;

    public string StoreCode { get; set; } = null!;

    public string? Sku { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Unit { get; set; } = null!;

    public double UnitPrice { get; set; }

    public string? MainImageUrl { get; set; }

    public int Status { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual RetailCategory Category { get; set; } = null!;
}
