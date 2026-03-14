namespace Category.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;

internal sealed class MaxDepthValidator : IBusinessOperationValidator<int>
{
    private const int MaxDepth = 4;

    public bool IsSatisfiedBy(int depth) => depth <= MaxDepth;

    public string ErrorMessage => $"Category depth must not exceed {MaxDepth}.";
}
