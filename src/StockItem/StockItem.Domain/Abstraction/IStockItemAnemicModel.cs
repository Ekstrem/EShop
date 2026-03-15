namespace StockItem.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IStockItemAnemicModel : IAnemicModel<IStockItem>
{
    IStockItemRoot Root { get; }
    IReadOnlyList<IReservation> Reservations { get; }
}
