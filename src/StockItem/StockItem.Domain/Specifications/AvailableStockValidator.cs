namespace StockItem.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;

internal sealed class AvailableStockValidator : IBusinessOperationValidator<(int Available, int Requested)>
{
    public bool IsSatisfiedBy(int available, int requested) => available >= requested;

    public bool IsSatisfiedBy((int Available, int Requested) value) => value.Available >= value.Requested;

    public string ErrorMessage => "Insufficient available stock for the requested quantity.";
}
