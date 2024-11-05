using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class VersionedApiController : ControllerBase
{
    /// <summary>
    /// Abstract BaseApi Controller Class
    /// </summary>
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}


public abstract class VersionedApiControllerBase<T> : VersionedApiController
{
    /// <summary>
    /// Abstract BaseApi Controller Class
    /// </summary>
    private ILogger<T>? _logger;
    protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
}
