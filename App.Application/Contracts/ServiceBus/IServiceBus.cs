using App.Domain.Event;

namespace App.Application.Contracts.ServiceBus
{
    public interface IServiceBus
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellation = default) where T : IEventOrMessage;

        Task SendAsync<T>(T message, string queueName, CancellationToken cancellation = default) where T : IEventOrMessage;
    }
}
