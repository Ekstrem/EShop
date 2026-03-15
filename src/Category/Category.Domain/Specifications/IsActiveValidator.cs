namespace Category.Domain.Specifications;

using Category.Domain.Abstraction;

internal sealed class IsActiveValidator
{
    public bool IsSatisfiedBy(ICategoryAnemicModel model)
        => model.Root.Status == "Active";

    public string Reason => "Category must be in Active status.";
}
