namespace StockItem.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;

internal sealed class NonNegativeReservedValidator : IBusinessOperationValidator<int>
{
    public bool IsSatisfiedBy(int reserved) => reserved >= 0;

    public string ErrorMessage => "Reserved quantity cannot be negative.";
}
