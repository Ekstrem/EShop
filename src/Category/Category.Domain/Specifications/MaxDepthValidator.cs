namespace Category.Domain.Specifications;

internal sealed class MaxDepthValidator
{
    private const int MaxDepth = 4;

    public bool IsSatisfiedBy(int depth) => depth <= MaxDepth;

    public string Reason => $"Category depth must not exceed {MaxDepth}.";
}
