namespace Cart.Domain.Specifications;

using Cart.Domain.Abstraction;

public class QuantityRangeValidator
{
    private const int MinQuantity = 1;
    private const int MaxQuantity = 99;

    private readonly int _quantity;

    public QuantityRangeValidator(int quantity)
    {
        _quantity = quantity;
    }

    public bool IsSatisfiedBy(ICartAnemicModel model)
        => _quantity >= MinQuantity && _quantity <= MaxQuantity;

    public string Reason => $"Quantity must be between {MinQuantity} and {MaxQuantity}.";
}
