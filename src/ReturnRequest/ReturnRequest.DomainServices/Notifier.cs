using Hive.SeedWorks.Events;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.DomainServices;

/// <summary>
/// Handles notifications for the ReturnRequest context domain events.
/// </summary>
public sealed class Notifier : INotifier<IReturnRequest>
{
    public void Notify<TModel>(AggregateResult<IReturnRequest, TModel> result)
        where TModel : IAnemicModel<IReturnRequest>
    {
        // Send notifications based on domain events.
    }
}
