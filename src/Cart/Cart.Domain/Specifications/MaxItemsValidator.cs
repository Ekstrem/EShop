namespace Cart.Domain.Specifications;

using Cart.Domain.Abstraction;

public class MaxItemsValidator
{
    private const int MaxItems = 50;

    public bool IsSatisfiedBy(ICartAnemicModel model)
        => model.Items.Count < MaxItems;

    public string Reason => $"Cart cannot contain more than {MaxItems} items.";
}
