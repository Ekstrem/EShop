using Hive.SeedWorks.TacticalPatterns;
using Review.Domain.Abstraction;

namespace Review.Domain.Specifications;

/// <summary>
/// Validates that the review has not been deleted.
/// </summary>
internal sealed class IsNotDeletedValidator : IBusinessOperationValidator<IReviewAnemicModel>
{
    public bool IsSatisfiedBy(IReviewAnemicModel model)
        => model.Root.Status != "Deleted";

    public string ErrorMessage => "Review has already been deleted.";
}
