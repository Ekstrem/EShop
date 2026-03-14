namespace Category.DomainServices;

using Hive.SeedWorks.Events;
using Category.Domain;

public sealed class BusAdapter : IBusAdapter<ICategory>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<ICategory>
    {
        return Task.CompletedTask;
    }
}
