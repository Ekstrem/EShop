namespace Order.Domain.Specifications;

using Order.Domain.Abstraction;

public class IsShippedValidator
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Root.Status == "Shipped";

    public string Reason => "Order must be in Shipped status.";
}
