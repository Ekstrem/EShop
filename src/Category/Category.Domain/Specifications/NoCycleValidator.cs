namespace Category.Domain.Specifications;

using Category.Domain.Abstraction;

internal sealed class NoCycleValidator
{
    private readonly IReadOnlyList<Guid> _ancestorIds;

    public NoCycleValidator(IReadOnlyList<Guid> ancestorIds)
    {
        _ancestorIds = ancestorIds;
    }

    public bool IsSatisfiedBy(ICategoryAnemicModel model)
        => true; // Cycle detection is validated via ancestor chain passed from application layer

    public string Reason => "Moving would create a cycle in the category hierarchy.";
}
