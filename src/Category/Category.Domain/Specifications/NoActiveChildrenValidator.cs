namespace Category.Domain.Specifications;

internal sealed class NoActiveChildrenValidator
{
    public bool IsSatisfiedBy(bool hasActiveChildren) => !hasActiveChildren;

    public string Reason => "Cannot deactivate a category with active children.";
}
