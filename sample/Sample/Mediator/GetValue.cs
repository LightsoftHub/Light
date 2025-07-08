using Sample.Mediator.Abstractions;

namespace Sample.Mediator;

public class GetValue
{
    public record Query : Light.Mediator.IQuery<Guid>;

    public class Handler : Light.Mediator.IQueryHandler<Query, Guid>
    {
        public Task<Guid> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = Guid.NewGuid();
            return Task.FromResult(result);
        }
    }
}
