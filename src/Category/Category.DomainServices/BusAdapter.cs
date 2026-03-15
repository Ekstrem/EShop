namespace Category.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using Category.Domain;

public sealed class BusAdapter : IBusAdapter<ICategory>
{
    public Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
