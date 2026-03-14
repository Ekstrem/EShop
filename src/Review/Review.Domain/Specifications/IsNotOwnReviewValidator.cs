using Hive.SeedWorks.TacticalPatterns;
using Review.Domain.Abstraction;

namespace Review.Domain.Specifications;

/// <summary>
/// Validates that the user is not the author of the review (for voting/flagging).
/// </summary>
internal sealed class IsNotOwnReviewValidator : IBusinessOperationValidator<IReviewAnemicModel>
{
    private readonly Guid _userId;

    private IsNotOwnReviewValidator(Guid userId)
    {
        _userId = userId;
    }

    public static IsNotOwnReviewValidator CreateInstance(Guid userId)
    {
        return new IsNotOwnReviewValidator(userId);
    }

    public bool IsSatisfiedBy(IReviewAnemicModel model)
        => model.Root.CustomerId != _userId;

    public string ErrorMessage => "Cannot perform this operation on your own review.";
}
