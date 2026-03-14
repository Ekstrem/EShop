namespace Product.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Product.Domain.Abstraction;

internal sealed class VariantHasPriceValidator : IBusinessOperationValidator<IProductVariant>
{
    public bool IsSatisfiedBy(IProductVariant variant)
        => variant.Price > 0;

    public string ErrorMessage => "Variant price must be greater than zero.";
}
