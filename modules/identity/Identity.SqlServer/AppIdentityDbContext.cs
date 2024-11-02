namespace Light.Identity.SqlServer;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : IdentityDbContext(options)
{
    protected override string CurrentUserId => "123";

    protected override bool SoftDelete => true;

    protected override DateTimeOffset AuditTime => DateTimeOffset.UtcNow.AddDays(2);
}