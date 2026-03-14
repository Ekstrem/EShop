namespace Category.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;

internal sealed class UniqueNameAmongSiblingsValidator : IBusinessOperationValidator<string>
{
    private readonly IReadOnlyList<string> _siblingNames;

    public UniqueNameAmongSiblingsValidator(IReadOnlyList<string> siblingNames)
    {
        _siblingNames = siblingNames;
    }

    public bool IsSatisfiedBy(string name)
        => !_siblingNames.Any(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase));

    public string ErrorMessage => "Category name must be unique among siblings.";
}
