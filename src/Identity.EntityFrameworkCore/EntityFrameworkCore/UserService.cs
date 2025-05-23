﻿using Microsoft.AspNetCore.Identity;

namespace Light.Identity.EntityFrameworkCore;

public class UserService(UserManager<User> userManager) : IUserService
{
    protected UserManager<User> UserManager => userManager;

    public virtual async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await userManager.Users
            .AsNoTracking()
            .OrderByDescending(x => x.Created)
            .ThenBy(x => x.UserName)
            .MapToDto()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<IResult<UserDto>> GetByIdAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null)
            return Result<UserDto>.NotFound($"User {id} not found");

        var dto = user.MapToDto();
        dto.Roles = await userManager.GetRolesAsync(user);

        return Result<UserDto>.Success(dto);
    }

    public virtual async Task<IResult<UserDto>> GetByUserNameAsync(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);

        if (user == null)
            return Result<UserDto>.NotFound($"User {userName} not found");

        var dto = user.MapToDto();
        dto.Roles = await userManager.GetRolesAsync(user);

        return Result<UserDto>.Success(dto);
    }

    public virtual async Task<IResult<string>> CreateAsync(CreateUserRequest newUser)
    {
        var entity = new User
        {
            UserName = newUser.UserName,
            Email = newUser.Email,
            PhoneNumber = newUser.PhoneNumber,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            UseDomainPassword = newUser.UseDomainPassword,
        };

        var identityResult = !string.IsNullOrEmpty(newUser.Password)
            ? await userManager.CreateAsync(entity, newUser.Password)
            : await userManager.CreateAsync(entity);

        return identityResult.ToResult(entity.Id);
    }

    public virtual async Task<IResult> UpdateAsync(UserDto updateUser)
    {
        var user = await userManager.FindByIdAsync(updateUser.Id);

        if (user == null)
            return Result.NotFound($"User {updateUser.Id} not found");

        // update base info
        user.UpdateInfo(
            updateUser.FirstName,
            updateUser.LastName,
            updateUser.PhoneNumber,
            updateUser.Email);

        // update status
        user.UpdateStatus(updateUser.Status);

        // auth by Domain
        user.ConnectDomain(updateUser.UseDomainPassword);

        var updatedResult = await userManager.UpdateAsync(user);
        if (!updatedResult.Succeeded)
            return updatedResult.ToResult();

        var currentRoles = await userManager.GetRolesAsync(user);
        var addRoles = updateUser.Roles.Except(currentRoles);
        var removeRoles = currentRoles.Except(updateUser.Roles);

        if (addRoles.Any())
        {
            var addRole = await userManager.AddToRolesAsync(user, addRoles);
            if (!addRole.Succeeded)
                return addRole.ToResult();
        }

        if (removeRoles.Any())
        {
            var removeRole = await userManager.RemoveFromRolesAsync(user, removeRoles);
            if (!removeRole.Succeeded)
                return removeRole.ToResult();
        }

        return Result.Success();
    }

    public virtual async Task<IResult> DeleteAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null)
            return Result.NotFound($"User {id} not found");

        user.Delete();

        var identityResult = await userManager.DeleteAsync(user);

        return identityResult.ToResult();
    }

    public virtual async Task<IResult> ForcePasswordAsync(string id, string password)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null)
            return Result.NotFound($"User {id} not found");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var identityResult = await userManager.ResetPasswordAsync(user, token, password);

        return identityResult.ToResult();
    }
}
