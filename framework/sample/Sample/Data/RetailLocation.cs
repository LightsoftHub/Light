using Light.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Sample.Data;

public partial class RetailLocation : IEntity
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

    public DateTimeOffset CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModifiedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public virtual ICollection<RetailStore> RetailStores { get; } = new List<RetailStore>();
}
