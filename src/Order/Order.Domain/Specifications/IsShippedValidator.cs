namespace Order.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain.Abstraction;

public class IsShippedValidator : IBusinessOperationValidator<IOrder, IOrderAnemicModel>
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Root.Status == "Shipped";

    public string ErrorMessage => "Order must be in Shipped status.";
}
