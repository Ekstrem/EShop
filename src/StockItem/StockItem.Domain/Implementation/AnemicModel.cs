namespace StockItem.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using StockItem.Domain.Abstraction;

internal sealed class AnemicModel : IStockItemAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IStockItemRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IReservation> Reservations { get; internal set; } = new List<IReservation>();

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
