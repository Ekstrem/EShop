namespace Order.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain.Abstraction;

public class IsPlacedValidator : IBusinessOperationValidator<IOrder, IOrderAnemicModel>
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Root.Status == "Placed";

    public string ErrorMessage => "Order must be in Placed status.";
}
