using Review.Domain.Abstraction;

namespace Review.Domain.Specifications;

/// <summary>
/// Validates that only one review exists per customer and product combination.
/// </summary>
internal sealed class UniqueReviewValidator
{
    private readonly Func<Guid, Guid, bool> _reviewExistsCheck;

    private UniqueReviewValidator(Func<Guid, Guid, bool> reviewExistsCheck)
    {
        _reviewExistsCheck = reviewExistsCheck;
    }

    public static UniqueReviewValidator CreateInstance(Func<Guid, Guid, bool> reviewExistsCheck)
    {
        return new UniqueReviewValidator(reviewExistsCheck);
    }

    public bool IsSatisfiedBy(IReviewAnemicModel model)
    {
        return !_reviewExistsCheck(model.Root.CustomerId, model.Root.ProductId);
    }

    public string Reason => "A review already exists for this customer and product combination.";
}
