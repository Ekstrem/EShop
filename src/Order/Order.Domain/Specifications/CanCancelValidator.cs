namespace Order.Domain.Specifications;

using Order.Domain.Abstraction;

public class CanCancelValidator
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Root.Status == "Placed" || model.Root.Status == "Paid";

    public string Reason => "Order can only be cancelled when in Placed or Paid status.";
}
