namespace Review.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using Review.Domain;

public sealed class BusAdapter : IBusAdapter<IReview>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
