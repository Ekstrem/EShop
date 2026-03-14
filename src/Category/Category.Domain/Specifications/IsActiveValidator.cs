namespace Category.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Category.Domain.Abstraction;

internal sealed class IsActiveValidator : IBusinessOperationValidator<ICategoryAnemicModel>
{
    public bool IsSatisfiedBy(ICategoryAnemicModel model)
        => model.Root.Status == "Active";

    public string ErrorMessage => "Category must be in Active status.";
}
