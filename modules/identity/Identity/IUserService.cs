namespace Light.Identity;

public interface IUserService
{
    /// <summary>
    /// Get all users
    /// </summary>
    Task<IResult<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get user by Id
    /// </summary>
    Task<IResult<UserDto>> GetByIdAsync(string id);

    /// <summary>
    /// Get user by UserName
    /// </summary>
    Task<IResult<UserDto>> GetByUserNameAsync(string userName);

    /// <summary>
    /// Validate user by ID & Password
    /// </summary>
    Task<IResult> CheckPasswordAsync(string id, string password);

    /// <summary>
    /// Validate user by UserName & Password
    /// </summary>
    Task<IResult> CheckPasswordByUserNameAsync(string userName, string password);

    /// <summary>
    /// Create user
    /// </summary>
    Task<IResult<string>> CreateAsync(CreateUserRequest newUser);

    /// <summary>
    /// Update user
    /// </summary>
    Task<IResult> UpdateAsync(UserDto updateUser);

    /// <summary>
    /// Delete user
    /// </summary>
    Task<IResult> DeleteAsync(string id);

    /// <summary>
    /// Force change password with auto generate reset token for user 
    /// </summary>
    Task<IResult> ForcePasswordAsync(string id, string password);
}