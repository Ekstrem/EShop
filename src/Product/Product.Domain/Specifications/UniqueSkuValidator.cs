namespace Product.Domain.Specifications;

using Product.Domain.Abstraction;

internal sealed class UniqueSkuValidator
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Variants
            .GroupBy(v => v.Sku)
            .All(g => g.Count() == 1);

    public string Reason => "Each variant must have a unique SKU.";
}
