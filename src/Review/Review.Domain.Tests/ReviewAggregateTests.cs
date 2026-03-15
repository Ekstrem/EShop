using EShop.Contracts;
using Review.Domain.Implementation;
using Xunit;

namespace Review.Domain.Tests;

public sealed class ReviewAggregateTests
{
    private static ReviewAggregate CreateEmptyAggregate()
        => ReviewAggregate.CreateInstance(new ReviewAnemicModel());

    private static ReviewAggregate CreatePublishedAggregate(
        Guid? customerId = null,
        Guid? productId = null)
    {
        var cid = customerId ?? Guid.NewGuid();
        var pid = productId ?? Guid.NewGuid();
        var root = ReviewRoot.CreateInstance(
            Guid.NewGuid(), pid, cid, 4, "Great product",
            "This is a really great product that I love using every day.",
            true, "Published", DateTime.UtcNow);

        var model = new ReviewAnemicModel
        {
            Root = root,
            HelpfulVotes = new List<Abstraction.IHelpfulVote>(),
            Flags = new List<Abstraction.IFlag>()
        };

        return ReviewAggregate.CreateInstance(model);
    }

    [Fact]
    public void SubmitReview_WithValidData_Succeeds()
    {
        var aggregate = CreateEmptyAggregate();
        var result = aggregate.SubmitReview(
            Guid.NewGuid(), Guid.NewGuid(), 5, "Excellent",
            "This product exceeded all my expectations and works perfectly.",
            true, (_, _) => false);

        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void SubmitReview_WithDuplicateReview_Fails()
    {
        var aggregate = CreateEmptyAggregate();
        var result = aggregate.SubmitReview(
            Guid.NewGuid(), Guid.NewGuid(), 5, "Excellent",
            "This product exceeded all my expectations and works perfectly.",
            true, (_, _) => true);

        Assert.False(result.IsSuccess());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void SubmitReview_WithInvalidRating_Fails(int rating)
    {
        var aggregate = CreateEmptyAggregate();
        var result = aggregate.SubmitReview(
            Guid.NewGuid(), Guid.NewGuid(), rating, "Title",
            "This product is okay but I have some issues with it overall.",
            false, (_, _) => false);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public void SubmitReview_WithShortText_Fails()
    {
        var aggregate = CreateEmptyAggregate();
        var result = aggregate.SubmitReview(
            Guid.NewGuid(), Guid.NewGuid(), 3, "Title",
            "Short",
            false, (_, _) => false);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public void EditReview_ByAuthor_WhenPublished_Succeeds()
    {
        var customerId = Guid.NewGuid();
        var aggregate = CreatePublishedAggregate(customerId);

        var result = aggregate.EditReview(
            customerId, 3, "Updated Title",
            "Updated review text that meets the minimum length requirement.");

        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void EditReview_ByNonAuthor_Fails()
    {
        var aggregate = CreatePublishedAggregate();

        var result = aggregate.EditReview(
            Guid.NewGuid(), 3, "Updated Title",
            "Updated review text that meets the minimum length requirement.");

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public void DeleteReview_Succeeds()
    {
        var aggregate = CreatePublishedAggregate();
        var result = aggregate.DeleteReview(Guid.NewGuid());

        Assert.True(result.IsSuccess());
        Assert.Equal("Deleted", result.Model().Root.Status);
    }

    [Fact]
    public void ApproveReview_FromSubmitted_Succeeds()
    {
        var root = ReviewRoot.CreateInstance(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 4, "Good",
            "This product is really good and I recommend it to everyone.",
            true, "Submitted", DateTime.UtcNow);
        var model = new ReviewAnemicModel
        {
            Root = root,
            HelpfulVotes = new List<Abstraction.IHelpfulVote>(),
            Flags = new List<Abstraction.IFlag>()
        };
        var aggregate = ReviewAggregate.CreateInstance(model);

        var result = aggregate.ApproveReview();

        Assert.True(result.IsSuccess());
        Assert.Equal("Published", result.Model().Root.Status);
    }

    [Fact]
    public void RejectReview_FromSubmitted_Succeeds()
    {
        var root = ReviewRoot.CreateInstance(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, "Bad",
            "This product is terrible and I do not recommend it at all.",
            false, "Submitted", DateTime.UtcNow);
        var model = new ReviewAnemicModel
        {
            Root = root,
            HelpfulVotes = new List<Abstraction.IHelpfulVote>(),
            Flags = new List<Abstraction.IFlag>()
        };
        var aggregate = ReviewAggregate.CreateInstance(model);

        var result = aggregate.RejectReview();

        Assert.True(result.IsSuccess());
        Assert.Equal("Rejected", result.Model().Root.Status);
    }

    [Fact]
    public void FlagReview_ByDifferentUser_Succeeds()
    {
        var aggregate = CreatePublishedAggregate();
        var flaggerId = Guid.NewGuid();

        var result = aggregate.FlagReview(flaggerId, "Inappropriate content");

        Assert.True(result.IsSuccess());
        Assert.Single(result.Model().Flags);
    }

    [Fact]
    public void FlagReview_ThreeFlags_ChangesStatusToFlagged()
    {
        var customerId = Guid.NewGuid();
        var aggregate = CreatePublishedAggregate(customerId);

        var flags = new List<Abstraction.IFlag>
        {
            Flag.CreateInstance(Guid.NewGuid(), "Spam", DateTime.UtcNow),
            Flag.CreateInstance(Guid.NewGuid(), "Inappropriate", DateTime.UtcNow)
        };

        var root = ReviewRoot.CreateInstance(
            aggregate.Model.Root.Id,
            aggregate.Model.Root.ProductId,
            customerId, 4, "Great product",
            "This is a really great product that I love using every day.",
            true, "Published", DateTime.UtcNow);
        var model = new ReviewAnemicModel
        {
            Root = root,
            HelpfulVotes = new List<Abstraction.IHelpfulVote>(),
            Flags = flags
        };
        var aggregateWithFlags = ReviewAggregate.CreateInstance(model);

        var result = aggregateWithFlags.FlagReview(Guid.NewGuid(), "Offensive");

        Assert.True(result.IsSuccess());
        Assert.Equal("Flagged", result.Model().Root.Status);
    }

    [Fact]
    public void VoteHelpful_ByDifferentUser_Succeeds()
    {
        var aggregate = CreatePublishedAggregate();
        var voterId = Guid.NewGuid();

        var result = aggregate.VoteHelpful(voterId);

        Assert.True(result.IsSuccess());
        Assert.Equal(1, result.Model().HelpfulCount);
    }

    [Fact]
    public void VoteHelpful_ByAuthor_Fails()
    {
        var customerId = Guid.NewGuid();
        var aggregate = CreatePublishedAggregate(customerId);

        var result = aggregate.VoteHelpful(customerId);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public void RespondToReview_WhenPublished_Succeeds()
    {
        var aggregate = CreatePublishedAggregate();

        var result = aggregate.RespondToReview("Thank you for your feedback!");

        Assert.True(result.IsSuccess());
        Assert.Equal("Thank you for your feedback!", result.Model().Root.ModeratorResponse);
    }
}
