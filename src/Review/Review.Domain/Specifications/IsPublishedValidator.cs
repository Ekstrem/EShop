using Hive.SeedWorks.TacticalPatterns;
using Review.Domain.Abstraction;

namespace Review.Domain.Specifications;

/// <summary>
/// Validates that the review status is Published.
/// </summary>
internal sealed class IsPublishedValidator : IBusinessOperationValidator<IReviewAnemicModel>
{
    public bool IsSatisfiedBy(IReviewAnemicModel model)
        => model.Root.Status == "Published";

    public string ErrorMessage => "Review must be in Published status for this operation.";
}
