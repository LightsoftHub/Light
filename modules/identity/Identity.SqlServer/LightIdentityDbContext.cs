namespace Light.Identity.SqlServer;

internal class LightIdentityDbContext(DbContextOptions<LightIdentityDbContext> options) :
    IdentityDbContext(options)
{
    protected override bool SoftDelete => true;

    protected override DateTimeOffset AuditTime => DateTimeOffset.UtcNow.AddDays(2);
}