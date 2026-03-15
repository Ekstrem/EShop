namespace StockItem.Domain.Specifications;

internal sealed class NonNegativeTotalValidator
{
    public bool IsSatisfiedBy(int total) => total >= 0;

    public string Reason => "Total stock cannot be negative.";
}
