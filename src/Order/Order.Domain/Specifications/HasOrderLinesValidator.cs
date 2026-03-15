namespace Order.Domain.Specifications;

using Order.Domain.Abstraction;

public class HasOrderLinesValidator
{
    public bool IsSatisfiedBy(IOrderAnemicModel model)
        => model.Lines.Count > 0;

    public string Reason => "Order must have at least one order line.";
}
