namespace Category.DomainServices;

using Hive.SeedWorks.Events;
using Category.Domain;

public sealed class Notifier : INotifier<ICategory>
{
    public Task Notify<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent<ICategory>
    {
        return Task.CompletedTask;
    }
}
