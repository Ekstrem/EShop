namespace Product.DomainServices;

using Hive.SeedWorks.Events;
using Product.Domain;

public sealed class BusAdapter : IBusAdapter<IProduct>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IProduct>
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
