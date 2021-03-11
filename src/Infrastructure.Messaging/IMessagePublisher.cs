using System.Threading.Tasks;

namespace Pitstop.Infrastructure.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishMessageAsync<IEvent>(IEvent message)
            where IEvent : Event;
    }
}
