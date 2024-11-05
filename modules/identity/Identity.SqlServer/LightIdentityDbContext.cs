namespace Light.Identity.SqlServer;

public class LightIdentityDbContext(DbContextOptions<LightIdentityDbContext> options) :
    IdentityDbContext(options)
{
    protected override bool SoftDelete => true;

    protected override string? TenantId => "A";

    protected override DateTimeOffset AuditTime => DateTimeOffset.UtcNow.AddDays(2);
}