using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Light.Identity.EntityFrameworkCore.Services;

public class RoleService(RoleManager<Role> roleManager) : IRoleService
{
    protected RoleManager<Role> RoleManager => roleManager;

    public virtual async Task<IResult<IEnumerable<RoleDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var roles = await roleManager.Roles
            .AsNoTracking()
            .MapToDto()
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<RoleDto>>.Success(roles);
    }

    public virtual async Task<IResult<RoleDto>> GetByIdAsync(string id)
    {
        var role = await roleManager.FindByIdAsync(id);

        if (role == null)
            return Result<RoleDto>.NotFound("Role", id);

        var result = role.MapToDto();

        var claims = await roleManager.GetClaimsAsync(role);

        result.Claims = claims.Select(s => new ClaimDto
        {
            Type = s.Type,
            Value = s.Value
        });

        return Result<RoleDto>.Success(result);
    }

    public virtual async Task<IResult<RoleDto>> GetByNameAsync(string name)
    {
        var role = await roleManager.FindByNameAsync(name);

        if (role == null)
            return Result<RoleDto>.NotFound("Role", name);

        var result = role.MapToDto();

        var claims = await roleManager.GetClaimsAsync(role);

        result.Claims = claims.Select(s => new ClaimDto
        {
            Type = s.Type,
            Value = s.Value
        });

        return Result<RoleDto>.Success(result);
    }

    public virtual async Task<IResult<string>> CreateAsync(CreateRoleRequest request)
    {
        var role = new Role
        {
            Name = request.Name,
            Description = request.Description,
        };

        var result = await roleManager.CreateAsync(role);

        return result.ToResult(role.Id);
    }

    public virtual async Task<IResult> UpdateAsync(RoleDto request)
    {
        var role = await roleManager.FindByIdAsync(request.Id);

        if (role == null)
            return Result.NotFound("Role", request.Id);

        role.Update(request.Name, request.Description);

        var result = await roleManager.UpdateAsync(role);

        // get claims of role
        var roleClaims = await roleManager.GetClaimsAsync(role);

        // remove claims not in request list
        foreach (var roleClaim in roleClaims)
        {
            // hold this claim if claim contains in request update list
            var hold = request.Claims.Any(a => a.Type == roleClaim.Type && a.Value == roleClaim.Value);

            if (hold) continue;

            // remove claim if not in request update list
            await roleManager.RemoveClaimAsync(role, roleClaim);
        }

        // add new claims in request list & skip exist claims
        foreach (var claim in request.Claims)
        {
            var owned = roleClaims.Any(a => a.Type == claim.Type && a.Value == claim.Value);

            if (owned) continue;

            // add new claim if role has not own it
            var newClaim = new Claim(claim.Type, claim.Value);
            await roleManager.AddClaimAsync(role, newClaim);
        }

        return result.ToResult();
    }

    public virtual async Task<IResult> DeleteAsync(string id)
    {
        var role = await roleManager.FindByIdAsync(id);

        if (role == null)
            return Result.NotFound("Role", id);

        //Check claim exist for role
        var claimsByRole = await roleManager.GetClaimsAsync(role);

        if (claimsByRole.Any())
            return Result.Error("Role has already setup claim.");

        var result = await roleManager.DeleteAsync(role);

        return result.ToResult();
    }
}
