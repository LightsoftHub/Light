using MediatR;

namespace Light.Contracts;

/// <summary>
/// Execute a query for read data from Database
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<TResponse> : IRequest<TResponse> { }

public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{ }


/// <summary>
/// Execute a command Insert, Update, Delete affect on Database with return value
/// </summary>
public interface ICommand<TResponse> : IRequest<TResponse> { }

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{ }

/// <summary>
/// Execute a command insert, update, delete affect on database without return value
/// </summary>
public interface ICommand : IRequest { }

public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest>
    where TRequest : ICommand
{ }