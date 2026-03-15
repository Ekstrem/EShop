namespace Order.Domain.Specifications;

using Order.Domain.Abstraction;

public class IsPaidValidator
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Root.Status == "Paid";

    public string Reason => "Order must be in Paid status.";
}
