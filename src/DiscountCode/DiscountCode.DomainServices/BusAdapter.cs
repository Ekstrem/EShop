namespace DiscountCode.DomainServices;

using Hive.SeedWorks.Events;
using DiscountCode.Domain;

public sealed class BusAdapter : IBusAdapter<IDiscountCode>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IDiscountCode>
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
