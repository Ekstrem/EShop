namespace Review.DomainServices;

using Hive.SeedWorks.Events;
using Review.Domain;

public sealed class BusAdapter : IBusAdapter<IReview>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<IReview>
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
