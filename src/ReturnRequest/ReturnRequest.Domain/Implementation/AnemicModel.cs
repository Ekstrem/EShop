using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Implementation;

/// <summary>
/// Anemic model implementation for the ReturnRequest aggregate.
/// </summary>
internal sealed class AnemicModel : AnemicModel<IReturnRequest>, IReturnRequestAnemicModel
{
    public IReturnRequestRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IReturnItem> Items { get; internal set; } = new List<IReturnItem>();
    public IReturnLabel? ReturnLabel { get; internal set; }
    public decimal RefundAmount { get; internal set; }
}
