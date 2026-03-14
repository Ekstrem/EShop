namespace StockItem.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IStockItemAnemicModel : IAnemicModel<IStockItem>
{
    IStockItemRoot Root { get; }
    IReadOnlyList<IReservation> Reservations { get; }
}
