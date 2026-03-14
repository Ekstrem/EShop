namespace Category.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Category.Domain.Abstraction;

internal sealed class IsInactiveValidator : IBusinessOperationValidator<ICategoryAnemicModel>
{
    public bool IsSatisfiedBy(ICategoryAnemicModel model)
        => model.Root.Status == "Inactive";

    public string ErrorMessage => "Category must be in Inactive status.";
}
