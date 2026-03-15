namespace Order.Domain.Specifications;

using Order.Domain.Abstraction;

public class OrderTotalNonNegativeValidator
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.OrderTotal.Total >= 0;

    public string Reason => "Order total must not be negative.";
}
