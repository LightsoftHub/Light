using Light.Mediator;

namespace Sample.Mediator;

public class GetValue
{
    public record Query : IQuery<Guid>;

    public class Handler : IQueryHandler<Query, Guid>
    {
        public Task<Guid> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = Guid.NewGuid();
            return Task.FromResult(result);
        }
    }
}
