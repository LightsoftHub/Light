using Light.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class TenantController(ITenantService tenantService) : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var res = await tenantService.GetAsync();
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertTenantRequest request)
    {
        var res = await tenantService.CreateAsync(request);
        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpsertTenantRequest request)
    {
        var res = await tenantService.UpdateAsync(id, request);
        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var res = await tenantService.DeleteAsync(id);
        return Ok(res);
    }
}