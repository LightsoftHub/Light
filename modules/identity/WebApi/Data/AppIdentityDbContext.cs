using Light.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : IdentityDbContext(options)
{
    protected override string CurrentUserId => "123";

    protected override bool SoftDelete => true;

    protected override DateTimeOffset Time => DateTimeOffset.UtcNow.AddDays(2);
}