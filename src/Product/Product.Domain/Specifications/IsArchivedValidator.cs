namespace Product.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Product.Domain.Abstraction;

internal sealed class IsArchivedValidator : IBusinessOperationValidator<IProductAnemicModel>
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Root.Status == "Archived";

    public string ErrorMessage => "Product must be in Archived status.";
}
