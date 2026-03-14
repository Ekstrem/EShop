namespace Category.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;

internal sealed class NoActiveChildrenValidator : IBusinessOperationValidator<bool>
{
    public bool IsSatisfiedBy(bool hasActiveChildren) => !hasActiveChildren;

    public string ErrorMessage => "Cannot deactivate a category with active children.";
}
