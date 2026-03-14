namespace Product.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Product.Domain.Abstraction;

internal sealed class IsPublishedValidator : IBusinessOperationValidator<IProductAnemicModel>
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Root.Status == "Published";

    public string ErrorMessage => "Product must be in Published status.";
}
