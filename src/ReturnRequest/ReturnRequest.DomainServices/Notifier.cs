using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.DomainServices;

/// <summary>
/// Handles notifications for the ReturnRequest context domain events.
/// </summary>
public sealed class Notifier
{
    public Task NotifyAsync(
        AggregateResult<IReturnRequest, IReturnRequestAnemicModel> result,
        CancellationToken cancellationToken = default)
    {
        // Send notifications based on domain events.
        return Task.CompletedTask;
    }
}
