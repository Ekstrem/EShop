namespace Hive.SeedWorks.Events;

using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;

/// <summary>Marker interface for domain events.</summary>
public interface IDomainEvent<TBC> where TBC : IBoundedContext { }

/// <summary>Event bus for publishing domain events.</summary>
public interface IEventBus
{
    Task Publish<TBC, TModel>(AggregateResult<TBC, TModel> result, CancellationToken ct = default)
        where TBC : IBoundedContext
        where TModel : IAnemicModel<TBC>;
}

/// <summary>Bus adapter for a specific bounded context.</summary>
public interface IBusAdapter<TBC> where TBC : IBoundedContext
{
    Task Publish<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : IDomainEvent<TBC>;
}

/// <summary>Notifier for aggregate result distribution.</summary>
public interface INotifier<TBC> where TBC : IBoundedContext
{
    void Notify<TModel>(AggregateResult<TBC, TModel> result)
        where TModel : IAnemicModel<TBC>;
}
