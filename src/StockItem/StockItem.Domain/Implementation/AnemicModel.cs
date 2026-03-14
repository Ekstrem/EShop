namespace StockItem.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using StockItem.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<IStockItem>, IStockItemAnemicModel
{
    public IStockItemRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IReservation> Reservations { get; internal set; } = new List<IReservation>();
}
