namespace Product.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Product.Domain.Abstraction;

internal sealed class UniqueSkuValidator : IBusinessOperationValidator<IProductAnemicModel>
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Variants
            .GroupBy(v => v.Sku)
            .All(g => g.Count() == 1);

    public string ErrorMessage => "Each variant must have a unique SKU.";
}
