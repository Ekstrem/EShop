namespace Product.Domain.Specifications;

using Product.Domain.Abstraction;

internal sealed class VariantHasPriceValidator
{
    public bool IsSatisfiedBy(IProductVariant variant)
        => variant.Price > 0;

    public string Reason => "Variant price must be greater than zero.";
}
