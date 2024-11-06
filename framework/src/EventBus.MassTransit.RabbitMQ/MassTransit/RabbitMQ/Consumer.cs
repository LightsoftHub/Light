using Light.EventBus.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Light.MassTransit.RabbitMQ
{
    public abstract class Consumer<TMessage> : IConsumer<TMessage>
         where TMessage : class, IIntegrationEvent 
    {
        private readonly ILogger _logger;

        public Consumer(ILogger logger)
        {
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<TMessage> context)
        {
            var message = context.Message;

            _logger.LogInformation("event_bus {id} consumed with data: {@Data}",
                message.Id,
                message);

            await Handle(message);
        }

        public abstract Task Handle(TMessage message);
    }
}
