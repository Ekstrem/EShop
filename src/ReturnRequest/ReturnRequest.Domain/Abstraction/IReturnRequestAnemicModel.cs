using Hive.SeedWorks.TacticalPatterns;

namespace ReturnRequest.Domain.Abstraction;

/// <summary>
/// Anemic model contract for the ReturnRequest aggregate.
/// </summary>
public interface IReturnRequestAnemicModel : IAnemicModel<IReturnRequest>
{
    IReturnRequestRoot Root { get; }
    IReadOnlyList<IReturnItem> Items { get; }
    IReturnLabel? ReturnLabel { get; }
    decimal RefundAmount { get; }
}
