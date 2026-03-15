using Review.Domain.Abstraction;

namespace Review.Domain.Specifications;

/// <summary>
/// Validates that the rating is between 1 and 5.
/// </summary>
internal sealed class RatingRangeValidator
{
    public bool IsSatisfiedBy(IReviewAnemicModel model)
        => model.Root.Rating >= 1 && model.Root.Rating <= 5;

    public string Reason => "Rating must be between 1 and 5.";
}
