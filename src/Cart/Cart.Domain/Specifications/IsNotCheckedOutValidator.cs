namespace Cart.Domain.Specifications;

using Cart.Domain.Abstraction;

public class IsNotCheckedOutValidator
{
    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.Root.Status != "CheckedOut";

    public string Reason => "Cart has already been checked out.";
}
