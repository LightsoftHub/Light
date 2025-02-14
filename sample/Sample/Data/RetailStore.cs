using Light.Domain.Entities;

namespace Sample.Data;

public partial class RetailStore : BaseAuditableEntity
{
    public string Code { get; set; } = null!;

    public string LocationCode { get; set; } = null!;

    public int Type { get; set; }

    public string Name { get; set; } = null!;

    public string? Note { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public int Status { get; set; }

    public virtual RetailLocation LocationCodeNavigation { get; set; } = null!;
}
