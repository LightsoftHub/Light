using Light.Contracts;
using Light.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Light.AspNetCore.Hosting.Extensions;

/// <summary>
/// Extensions for result to Api Response
/// </summary>
public static class ApiResponseExtensions
{
    /// <summary>
    /// Return result with status code sames result code
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static IActionResult ToActionResult(this IResult result)
    {
        return new ObjectResult(result)
        {
            StatusCode = (int)result.MapHttpStatusCode()
        };
    }
}
