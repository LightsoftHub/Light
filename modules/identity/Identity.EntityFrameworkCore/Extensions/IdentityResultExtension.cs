namespace Light.Identity.EntityFrameworkCore.Extensions;

public static class IdentityResultExtensions
{
    public static Result ToResult(this Microsoft.AspNetCore.Identity.IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Error(errors: result.Errors.Select(e => e.Description));
    }

    public static Result<T> ToResult<T>(this Microsoft.AspNetCore.Identity.IdentityResult result, T data)
    {
        return result.Succeeded
            ? Result<T>.Success(data: data)
            : Result<T>.Error(errors: result.Errors.Select(e => e.Description));
    }
}