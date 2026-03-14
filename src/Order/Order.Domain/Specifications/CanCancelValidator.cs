namespace Order.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain.Abstraction;

public class CanCancelValidator : IBusinessOperationValidator<IOrder, IOrderAnemicModel>
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Root.Status == "Placed" || model.Root.Status == "Paid";

    public string ErrorMessage => "Order can only be cancelled when in Placed or Paid status.";
}
