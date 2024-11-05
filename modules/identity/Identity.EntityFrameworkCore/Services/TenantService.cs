using Light.EntityFrameworkCore.Extensions;
using Light.Identity.EntityFrameworkCore;

namespace Light.Identity.Services;

public class TenantService(IIdentityDbContext context) : ITenantService
{
    public Task<Result<IEnumerable<TenantDto>>> GetAsync()
    {
        return context.Tenants
            .Select(s => new TenantDto(s.Id, s.Name))
            .AsNoTracking()
            .ToListResultAsync();
    }

    public async Task<Result<string>> CreateAsync(UpsertTenantRequest request)
    {
        var entity = new Tenant()
        {
            Name = request.Name,
        };

        await context.Tenants.AddAsync(entity);
        await context.SaveChangesAsync();

        return Result<string>.Success(entity.Id);
    }

    public async Task<Result> UpdateAsync(string id, UpsertTenantRequest request)
    {
        var tenant = await context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return Result.NotFound<Tenant>(id);
        }

        tenant.Name = request.Name;

        await context.SaveChangesAsync();

        return Result.Success(tenant.Id);
    }

    public async Task<Result> DeleteAsync(string id)
    {
        //await context.Tenants.Where(x => x.Id == id).ExecuteDeleteAsync();

        var tenant = await context.Tenants.FindAsync(id);

        if (tenant == null)
        {
            return Result.NotFound<Tenant>(id);
        }

        context.Tenants.Remove(tenant);
        await context.SaveChangesAsync();

        return Result.Success();
    }
}
