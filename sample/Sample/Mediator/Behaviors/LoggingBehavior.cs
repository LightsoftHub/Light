using Light.Mediator;

namespace Sample.Mediator.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
    {
        var timer = System.Diagnostics.Stopwatch.StartNew();

        logger.LogInformation("Handling {RequestType}", typeof(TRequest).FullName);

        var response = await next();

        timer.Stop();

        logger.LogInformation("Done handling {RequestType} in {ms}", typeof(TRequest).FullName, timer.ElapsedMilliseconds);
        
        return response;
    }
}