using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using Review.Domain.Abstraction;
using Review.Domain.Specifications;

namespace Review.Domain.Implementation;

/// <summary>
/// Review aggregate containing all business operations.
/// Each method returns an AggregateResult for event-driven processing.
/// </summary>
public sealed class ReviewAggregate : Aggregate<IReview, IReviewAnemicModel>
{
    private ReviewAggregate(IReviewAnemicModel model) : base(model) { }

    public static ReviewAggregate CreateInstance(IReviewAnemicModel model) => new(model);

    /// <summary>
    /// Submits a new review. Validates uniqueness per customer+product, rating range, and text length.
    /// </summary>
    public AggregateResult<IReview, IReviewAnemicModel> SubmitReview(
        Guid productId,
        Guid customerId,
        int rating,
        string title,
        string text,
        bool isVerifiedPurchase,
        Func<Guid, Guid, bool> reviewExistsCheck)
    {
        var uniqueValidator = UniqueReviewValidator.CreateInstance(reviewExistsCheck);
        var tempRoot = ReviewRoot.CreateInstance(
            Guid.NewGuid(), productId, customerId, rating, title, text,
            isVerifiedPurchase, "Submitted", DateTime.UtcNow);
        var tempModel = new ReviewAnemicModel
        {
            Root = tempRoot,
            HelpfulVotes = new List<IHelpfulVote>(),
            Flags = new List<IFlag>()
        };

        if (!uniqueValidator.IsSatisfiedBy(tempModel))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(uniqueValidator.ErrorMessage);

        var ratingValidator = new RatingRangeValidator();
        if (!ratingValidator.IsSatisfiedBy(tempModel))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(ratingValidator.ErrorMessage);

        var textValidator = new TextLengthValidator();
        if (!textValidator.IsSatisfiedBy(tempModel))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(textValidator.ErrorMessage);

        return AggregateResult<IReview, IReviewAnemicModel>.Ok(tempModel);
    }

    /// <summary>
    /// Edits an existing review. Only the author can edit, and only when Published.
    /// </summary>
    public AggregateResult<IReview, IReviewAnemicModel> EditReview(
        Guid requesterId,
        int rating,
        string title,
        string text)
    {
        var authorValidator = IsAuthorValidator.CreateInstance(requesterId);
        if (!authorValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(authorValidator.ErrorMessage);

        var publishedValidator = new IsPublishedValidator();
        if (!publishedValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(publishedValidator.ErrorMessage);

        var updatedRoot = ReviewRoot.CreateInstance(
            Model.Root.Id,
            Model.Root.ProductId,
            Model.Root.CustomerId,
            rating, title, text,
            Model.Root.IsVerifiedPurchase,
            Model.Root.Status,
            Model.Root.CreatedAt,
            Model.Root.ModeratorResponse);

        var tempModel = new ReviewAnemicModel
        {
            Root = updatedRoot,
            HelpfulVotes = Model.HelpfulVotes.ToList(),
            Flags = Model.Flags.ToList()
        };

        var ratingValidator = new RatingRangeValidator();
        if (!ratingValidator.IsSatisfiedBy(tempModel))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(ratingValidator.ErrorMessage);

        var textValidator = new TextLengthValidator();
        if (!textValidator.IsSatisfiedBy(tempModel))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(textValidator.ErrorMessage);

        return AggregateResult<IReview, IReviewAnemicModel>.Ok(tempModel);
    }

    /// <summary>
    /// Soft-deletes a review. Author or moderator can delete.
    /// </summary>
    public AggregateResult<IReview, IReviewAnemicModel> DeleteReview(Guid requesterId)
    {
        var deletedValidator = new IsNotDeletedValidator();
        if (!deletedValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(deletedValidator.ErrorMessage);

        var updatedRoot = ReviewRoot.CreateInstance(
            Model.Root.Id,
            Model.Root.ProductId,
            Model.Root.CustomerId,
            Model.Root.Rating,
            Model.Root.Title,
            Model.Root.Text,
            Model.Root.IsVerifiedPurchase,
            "Deleted",
            Model.Root.CreatedAt,
            Model.Root.ModeratorResponse);

        var anemic = new ReviewAnemicModel
        {
            Root = updatedRoot,
            HelpfulVotes = Model.HelpfulVotes.ToList(),
            Flags = Model.Flags.ToList()
        };

        return AggregateResult<IReview, IReviewAnemicModel>.Ok(anemic);
    }

    /// <summary>
    /// Approves a review. Transitions Submitted or Flagged to Published.
    /// </summary>
    public AggregateResult<IReview, IReviewAnemicModel> ApproveReview()
    {
        var statusValidator = new IsSubmittedOrFlaggedValidator();
        if (!statusValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(statusValidator.ErrorMessage);

        var updatedRoot = ReviewRoot.CreateInstance(
            Model.Root.Id,
            Model.Root.ProductId,
            Model.Root.CustomerId,
            Model.Root.Rating,
            Model.Root.Title,
            Model.Root.Text,
            Model.Root.IsVerifiedPurchase,
            "Published",
            Model.Root.CreatedAt,
            Model.Root.ModeratorResponse);

        var anemic = new ReviewAnemicModel
        {
            Root = updatedRoot,
            HelpfulVotes = Model.HelpfulVotes.ToList(),
            Flags = new List<IFlag>()
        };

        return AggregateResult<IReview, IReviewAnemicModel>.Ok(anemic);
    }

    /// <summary>
    /// Rejects a review. Transitions Submitted or Flagged to Rejected.
    /// </summary>
    public AggregateResult<IReview, IReviewAnemicModel> RejectReview()
    {
        var statusValidator = new IsSubmittedOrFlaggedValidator();
        if (!statusValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(statusValidator.ErrorMessage);

        var updatedRoot = ReviewRoot.CreateInstance(
            Model.Root.Id,
            Model.Root.ProductId,
            Model.Root.CustomerId,
            Model.Root.Rating,
            Model.Root.Title,
            Model.Root.Text,
            Model.Root.IsVerifiedPurchase,
            "Rejected",
            Model.Root.CreatedAt,
            Model.Root.ModeratorResponse);

        var anemic = new ReviewAnemicModel
        {
            Root = updatedRoot,
            HelpfulVotes = Model.HelpfulVotes.ToList(),
            Flags = Model.Flags.ToList()
        };

        return AggregateResult<IReview, IReviewAnemicModel>.Ok(anemic);
    }

    /// <summary>
    /// Flags a review. Must be Published, not own review. If 3+ flags, status becomes Flagged.
    /// </summary>
    public AggregateResult<IReview, IReviewAnemicModel> FlagReview(Guid flaggerId, string reason)
    {
        var publishedValidator = new IsPublishedValidator();
        if (!publishedValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(publishedValidator.ErrorMessage);

        var notOwnValidator = IsNotOwnReviewValidator.CreateInstance(flaggerId);
        if (!notOwnValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(notOwnValidator.ErrorMessage);

        var newFlag = Flag.CreateInstance(flaggerId, reason, DateTime.UtcNow);
        var flags = Model.Flags.ToList();
        flags.Add(newFlag);

        var newStatus = flags.Count >= 3 ? "Flagged" : Model.Root.Status;

        var updatedRoot = ReviewRoot.CreateInstance(
            Model.Root.Id,
            Model.Root.ProductId,
            Model.Root.CustomerId,
            Model.Root.Rating,
            Model.Root.Title,
            Model.Root.Text,
            Model.Root.IsVerifiedPurchase,
            newStatus,
            Model.Root.CreatedAt,
            Model.Root.ModeratorResponse);

        var anemic = new ReviewAnemicModel
        {
            Root = updatedRoot,
            HelpfulVotes = Model.HelpfulVotes.ToList(),
            Flags = flags
        };

        return AggregateResult<IReview, IReviewAnemicModel>.Ok(anemic);
    }

    /// <summary>
    /// Adds a helpful vote. Must be Published, not own review.
    /// </summary>
    public AggregateResult<IReview, IReviewAnemicModel> VoteHelpful(Guid voterId)
    {
        var publishedValidator = new IsPublishedValidator();
        if (!publishedValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(publishedValidator.ErrorMessage);

        var notOwnValidator = IsNotOwnReviewValidator.CreateInstance(voterId);
        if (!notOwnValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(notOwnValidator.ErrorMessage);

        if (Model.HelpfulVotes.Any(v => v.VoterId == voterId))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail("User has already voted on this review.");

        var vote = HelpfulVote.CreateInstance(voterId, DateTime.UtcNow);
        var votes = Model.HelpfulVotes.ToList();
        votes.Add(vote);

        var anemic = new ReviewAnemicModel
        {
            Root = Model.Root,
            HelpfulVotes = votes,
            Flags = Model.Flags.ToList()
        };

        return AggregateResult<IReview, IReviewAnemicModel>.Ok(anemic);
    }

    /// <summary>
    /// Adds a moderator response to a Published review.
    /// </summary>
    public AggregateResult<IReview, IReviewAnemicModel> RespondToReview(string moderatorResponse)
    {
        var publishedValidator = new IsPublishedValidator();
        if (!publishedValidator.IsSatisfiedBy(Model))
            return AggregateResult<IReview, IReviewAnemicModel>.Fail(publishedValidator.ErrorMessage);

        var updatedRoot = ReviewRoot.CreateInstance(
            Model.Root.Id,
            Model.Root.ProductId,
            Model.Root.CustomerId,
            Model.Root.Rating,
            Model.Root.Title,
            Model.Root.Text,
            Model.Root.IsVerifiedPurchase,
            Model.Root.Status,
            Model.Root.CreatedAt,
            moderatorResponse);

        var anemic = new ReviewAnemicModel
        {
            Root = updatedRoot,
            HelpfulVotes = Model.HelpfulVotes.ToList(),
            Flags = Model.Flags.ToList()
        };

        return AggregateResult<IReview, IReviewAnemicModel>.Ok(anemic);
    }
}
