namespace Promotion.DomainServices;

using Hive.SeedWorks.Events;
using Promotion.Domain;

public sealed class BusAdapter : IBusAdapter<IPromotion>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IPromotion>
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
