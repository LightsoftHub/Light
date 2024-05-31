using Light.Domain.Entities.Interfaces;

namespace Sample.Data;

public partial class RetailStore : IEntity
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

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual RetailLocation LocationCodeNavigation { get; set; } = null!;
}
