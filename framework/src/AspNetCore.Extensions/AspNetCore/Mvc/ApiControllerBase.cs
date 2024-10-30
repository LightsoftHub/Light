using Light.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Light.AspNetCore.Mvc;

/// <summary>
/// Abstract BaseApi Controller Class
/// </summary>

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    /// <summary>
    /// Default success response
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    public virtual IActionResult Ok(IResult result)
    {
        result.RequestId = HttpContext.TraceIdentifier;
        return result.ToActionResult();
    }
}
