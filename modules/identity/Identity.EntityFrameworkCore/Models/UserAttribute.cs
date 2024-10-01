namespace Light.Identity.EntityFrameworkCore.Models;

public class UserAttribute : Domain.Entities.AuditableEntity
{
    public string UserId { get; set; } = null!;

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;
}