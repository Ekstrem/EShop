namespace Cart.Domain.Specifications;

using Cart.Domain.Abstraction;

public class IsActiveCartValidator
{
    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.Root.Status == "Active";

    public string Reason => "Cart must be in Active status.";
}
