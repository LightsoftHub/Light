using Light.Extensions.DependencyInjection;

namespace Sample.Services
{
    public interface IDateTimeService : ISingletonDependency
    {
        DateTime Now { get; }
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
