namespace Order.Domain.Specifications;

using Order.Domain.Abstraction;

public class IsPlacedValidator
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Root.Status == "Placed";

    public string Reason => "Order must be in Placed status.";
}
