namespace Order.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain.Abstraction;

public class HasOrderLinesValidator : IBusinessOperationValidator<IOrder, IOrderAnemicModel>
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Lines.Count > 0;

    public string ErrorMessage => "Order must have at least one order line.";
}
