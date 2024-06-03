using Microsoft.EntityFrameworkCore;

namespace Light.Identity.Migrator.Data;

public class MigratorDbContext(
    DbContextOptions<MigratorDbContext> options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
