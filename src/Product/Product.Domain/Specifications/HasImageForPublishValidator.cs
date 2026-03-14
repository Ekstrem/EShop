namespace Product.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Product.Domain.Abstraction;

internal sealed class HasImageForPublishValidator : IBusinessOperationValidator<IProductAnemicModel>
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Media.Count >= 1;

    public string ErrorMessage => "Product must have at least one image to be published.";
}
