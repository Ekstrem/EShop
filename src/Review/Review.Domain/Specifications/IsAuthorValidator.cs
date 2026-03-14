using Hive.SeedWorks.TacticalPatterns;
using Review.Domain.Abstraction;

namespace Review.Domain.Specifications;

/// <summary>
/// Validates that the requester is the author of the review.
/// </summary>
internal sealed class IsAuthorValidator : IBusinessOperationValidator<IReviewAnemicModel>
{
    private readonly Guid _requesterId;

    private IsAuthorValidator(Guid requesterId)
    {
        _requesterId = requesterId;
    }

    public static IsAuthorValidator CreateInstance(Guid requesterId)
    {
        return new IsAuthorValidator(requesterId);
    }

    public bool IsSatisfiedBy(IReviewAnemicModel model)
        => model.Root.CustomerId == _requesterId;

    public string ErrorMessage => "Only the author can perform this operation.";
}
