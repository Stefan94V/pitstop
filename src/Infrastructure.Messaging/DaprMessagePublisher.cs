using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Pitstop.Infrastructure.Messaging
{
    public class DaprMessagePublisher : IMessagePublisher
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<DaprMessagePublisher> _logger;

        private const string PubSubName = "pubsub";


        public DaprMessagePublisher(DaprClient daprClient, ILogger<DaprMessagePublisher> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }
        
        public async Task PublishMessageAsync<IEvent>(IEvent message)
          where IEvent : Event
        {
            var messageType = message.GetType().Name;
            _logger.LogInformation("Publishing {MessageType}", messageType);
            await _daprClient.PublishEventAsync(PubSubName, messageType, (dynamic)message);
        }
    }
}