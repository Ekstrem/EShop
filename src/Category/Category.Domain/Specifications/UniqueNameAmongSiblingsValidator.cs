namespace Category.Domain.Specifications;

internal sealed class UniqueNameAmongSiblingsValidator
{
    private readonly IReadOnlyList<string> _siblingNames;

    public UniqueNameAmongSiblingsValidator(IReadOnlyList<string> siblingNames)
    {
        _siblingNames = siblingNames;
    }

    public bool IsSatisfiedBy(string name)
        => !_siblingNames.Any(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase));

    public string Reason => "Category name must be unique among siblings.";
}
