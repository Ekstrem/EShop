namespace Product.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Product.Domain.Abstraction;

internal sealed class IsDraftValidator : IBusinessOperationValidator<IProductAnemicModel>
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Root.Status == "Draft";

    public string ErrorMessage => "Product must be in Draft status.";
}
