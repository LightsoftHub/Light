using FluentValidation;
using Light.Contracts;
using Light.Mediator;

namespace Sample.Mediator;

public record CreateValueCommand(string Id) : ICommand<Result<string>>;

internal class CreateValueHandler(IPublisher publisher) : ICommandHandler<CreateValueCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateValueCommand request, CancellationToken cancellationToken)
    {
        var result = Result<string>.Success(request.Id);

        // publih the event
        await publisher.Publish(new ValueUpdated.Event(request.Id), cancellationToken);

        return result;
    }
}