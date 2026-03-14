namespace Campaign.DomainServices;

using Hive.SeedWorks.Events;
using Campaign.Domain;

public sealed class BusAdapter : IBusAdapter<ICampaign>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<ICampaign>
    {
        // Integration with event bus (RabbitMQ, Kafka, etc.)
        return Task.CompletedTask;
    }
}
