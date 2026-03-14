namespace Cart.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain.Abstraction;

public class IsActiveCartValidator : IBusinessOperationValidator<ICart, ICartAnemicModel>
{
    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.Root.Status == "Active";

    public string ErrorMessage => "Cart must be in Active status.";
}
