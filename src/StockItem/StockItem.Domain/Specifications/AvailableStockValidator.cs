namespace StockItem.Domain.Specifications;

internal sealed class AvailableStockValidator
{
    public bool IsSatisfiedBy(int available, int requested) => available >= requested;

    public string Reason => "Insufficient available stock for the requested quantity.";
}
