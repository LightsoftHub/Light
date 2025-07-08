using Light.Contracts;
using Light.Mediator;

namespace Sample.Mediator.Abstractions;

public interface ICommand : Light.Mediator.ICommand<Result>;

public interface ICommandHandler<in TRequest>
    : Light.Mediator.ICommandHandler<TRequest, Result>
    where TRequest : ICommand;
