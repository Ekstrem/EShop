namespace Product.Domain.Specifications;

using Product.Domain.Abstraction;

internal sealed class HasAtLeastOneVariantValidator
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Variants.Count >= 1;

    public string Reason => "Product must have at least one variant.";
}
