using Light.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sample.Data;

public partial class RetailLocation : BaseAuditableEntity
{
    [MinLength(10)]
    public string Code { get; set; } = null!;

    [MinLength(15)]
    public string Name { get; set; } = null!;

    [MinLength(5)]
    public string? Phone { get; set; }

    [MinLength(5)]
    [EmailAddress]
    public string? Address { get; set; }

    [MinLength(5)]
    public string? City { get; set; }

    [MinLength(5)]
    public string? Country { get; set; }

    public virtual ICollection<RetailStore> RetailStores { get; } = new List<RetailStore>();
}
