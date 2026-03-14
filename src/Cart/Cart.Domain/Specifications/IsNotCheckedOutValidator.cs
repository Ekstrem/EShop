namespace Cart.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain.Abstraction;

public class IsNotCheckedOutValidator : IBusinessOperationValidator<ICart, ICartAnemicModel>
{
    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.Root.Status != "CheckedOut";

    public string ErrorMessage => "Cart has already been checked out.";
}
