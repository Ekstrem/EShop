namespace Cart.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain.Abstraction;

public class QuantityRangeValidator : IBusinessOperationValidator<ICart, ICartAnemicModel>
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

    public string ErrorMessage => $"Quantity must be between {MinQuantity} and {MaxQuantity}.";
}
