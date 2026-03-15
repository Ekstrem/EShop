namespace AggregateRating.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using AggregateRating.Domain;

public sealed class BusAdapter : IBusAdapter<IAggregateRating>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
