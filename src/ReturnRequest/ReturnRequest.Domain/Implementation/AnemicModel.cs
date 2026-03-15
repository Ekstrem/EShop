using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Implementation;

/// <summary>
/// Anemic model implementation for the ReturnRequest aggregate.
/// </summary>
internal sealed class AnemicModel : IReturnRequestAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IReturnRequestRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IReturnItem> Items { get; internal set; } = new List<IReturnItem>();
    public IReturnLabel? ReturnLabel { get; internal set; }
    public decimal RefundAmount { get; internal set; }

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
