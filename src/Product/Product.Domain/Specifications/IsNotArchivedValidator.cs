namespace Product.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Product.Domain.Abstraction;

internal sealed class IsNotArchivedValidator : IBusinessOperationValidator<IProductAnemicModel>
{
    public bool IsSatisfiedBy(IProductAnemicModel model)
        => model.Root.Status != "Archived";

    public string ErrorMessage => "Product must not be archived for this operation.";
}
