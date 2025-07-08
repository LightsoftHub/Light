using FluentValidation;
using Light.Contracts;
using Light.Mediator;

namespace Sample.Mediator;

public class UpdateValue
{
    public record Command(string Id) : Light.Mediator.ICommand<Result<string>>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().MinimumLength(3);
        }
    }

    public class Handler(IPublisher publisher) : Light.Mediator.ICommandHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = Result<string>.Success(request.Id);

            // publih the event
            await publisher.Publish(new ValueUpdated.Event(request.Id), cancellationToken);

            return result;
        }
    }
}
