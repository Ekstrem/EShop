namespace Product.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using Product.Domain;

public sealed class BusAdapter : IBusAdapter<IProduct>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
