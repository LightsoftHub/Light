using Sample.Mediator.Abstractions;

namespace Sample.Mediator;

public class DeleteValue
{
    public record Command(string Id) : ICommand;

    internal class Handler(ILogger<Handler> logger) : ICommandHandler<Command>
    {
        public async Task<Light.Contracts.Result> Handle(Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Value with ID {Id} deleted.", request.Id);

            await Task.Yield(); // Simulate async operation

            return Light.Contracts.Result.Success();
        }
    }
}
