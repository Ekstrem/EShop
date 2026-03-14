namespace Notification.DomainServices;

using Hive.SeedWorks.Events;
using Notification.Domain;

public sealed class BusAdapter : IBusAdapter<INotification>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<INotification>
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
