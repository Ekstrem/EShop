namespace AggregateRating.DomainServices;

using Hive.SeedWorks.Events;
using AggregateRating.Domain;

public sealed class BusAdapter : IBusAdapter<IAggregateRating>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IAggregateRating>
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
