namespace StockItem.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;

internal sealed class NonNegativeTotalValidator : IBusinessOperationValidator<int>
{
    public bool IsSatisfiedBy(int total) => total >= 0;

    public string ErrorMessage => "Total stock cannot be negative.";
}
