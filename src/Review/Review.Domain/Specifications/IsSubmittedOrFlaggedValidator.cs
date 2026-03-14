using Hive.SeedWorks.TacticalPatterns;
using Review.Domain.Abstraction;

namespace Review.Domain.Specifications;

/// <summary>
/// Validates that the review status is Submitted or Flagged.
/// </summary>
internal sealed class IsSubmittedOrFlaggedValidator : IBusinessOperationValidator<IReviewAnemicModel>
{
    public bool IsSatisfiedBy(IReviewAnemicModel model)
        => model.Root.Status is "Submitted" or "Flagged";

    public string ErrorMessage => "Review must be in Submitted or Flagged status for this operation.";
}
