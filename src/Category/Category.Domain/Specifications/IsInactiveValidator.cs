namespace Category.Domain.Specifications;

using Category.Domain.Abstraction;

internal sealed class IsInactiveValidator
{
    public bool IsSatisfiedBy(ICategoryAnemicModel model)
        => model.Root.Status == "Inactive";

    public string Reason => "Category must be in Inactive status.";
}
