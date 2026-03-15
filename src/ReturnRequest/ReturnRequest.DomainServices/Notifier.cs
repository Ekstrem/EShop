using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
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
