namespace Light.Identity;

public interface ITenantService
{
    Task<Result<IEnumerable<TenantDto>>> GetAsync();

    Task<Result<string>> CreateAsync(UpsertTenantRequest request);

    Task<Result> UpdateAsync(string id, UpsertTenantRequest request);

    Task<Result> DeleteAsync(string id);
}
