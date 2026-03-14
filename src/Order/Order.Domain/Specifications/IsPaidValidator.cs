namespace Order.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain.Abstraction;

public class IsPaidValidator : IBusinessOperationValidator<IOrder, IOrderAnemicModel>
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Root.Status == "Paid";

    public string ErrorMessage => "Order must be in Paid status.";
}
