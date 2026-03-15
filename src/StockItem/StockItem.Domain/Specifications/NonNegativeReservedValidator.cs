namespace StockItem.Domain.Specifications;

internal sealed class NonNegativeReservedValidator
{
    public bool IsSatisfiedBy(int reserved) => reserved >= 0;

    public string Reason => "Reserved quantity cannot be negative.";
}
