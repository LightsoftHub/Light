using System.Threading;
using System.Threading.Tasks;

namespace Light.Mediator
{
    public interface IRequest<TResponse>
    { }

    public interface IRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}