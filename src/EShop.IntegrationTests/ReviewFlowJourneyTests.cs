using Xunit;
using Review.Domain.Abstraction;
using Review.Domain.Implementation;
using AggregateRating.Domain.Abstraction;
using AggregateRating.Domain.Implementation;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Implementation;

namespace EShop.IntegrationTests;

/// <summary>
/// Journey 8: Shipment → Review → AggregateRating
/// Verifies the post-delivery review flow: after a shipment is delivered,
/// a customer submits a review, the review is moderated (approved),
/// and the aggregate rating for the product is recalculated.
/// </summary>
public sealed class ReviewFlowJourneyTests
{
    [Fact]
    public void DeliveredShipment_SubmitReview_ApproveReview_RecalculateRating_Succeeds()
    {
        // Arrange — shared identifiers
        var productId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        // ── Step 1: Construct a delivered shipment ──────────────────────
        var shipmentRoot = ShipmentRoot.CreateInstance(
            Guid.NewGuid(),
            orderId,
            "TRACK-12345",
            "FedEx",
            "123 Main St, Springfield, IL 62701",
            "Delivered",
            DateTime.UtcNow.AddDays(-2));

        var shipmentItem = ShipmentItem.CreateInstance(variantId, "Widget Pro", 1);
        var shipmentLabel = ShippingLabel.CreateInstance(
            "https://labels.example.com/12345.pdf",
            DateTime.UtcNow.AddDays(-3));

        var shipmentModel = new AnemicModel
        {
            Root = shipmentRoot,
            Items = new List<IShipmentItem> { shipmentItem },
            Label = shipmentLabel
        };

        // Verify the shipment is delivered
        Assert.Equal("Delivered", shipmentModel.Root.Status);

        // ── Step 2: Submit a review for the product ─────────────────────
        var emptyReviewModel = new ReviewAnemicModel();
        var reviewAggregate = ReviewAggregate.CreateInstance(emptyReviewModel);

        var submitResult = reviewAggregate.SubmitReview(
            productId,
            customerId,
            rating: 5,
            title: "Excellent product!",
            text: "This product exceeded all my expectations. Build quality is top notch and it works perfectly.",
            isVerifiedPurchase: true,
            reviewExistsCheck: (_, _) => false);

        Assert.True(submitResult.IsSuccess, $"SubmitReview failed: {submitResult.ErrorMessage}");
        Assert.Equal("Submitted", submitResult.Model!.Root.Status);
        Assert.Equal(5, submitResult.Model.Root.Rating);
        Assert.True(submitResult.Model.Root.IsVerifiedPurchase);

        // ── Step 3: Approve the review (moderation) ─────────────────────
        var submittedAggregate = ReviewAggregate.CreateInstance(submitResult.Model);
        var approveResult = submittedAggregate.ApproveReview();

        Assert.True(approveResult.IsSuccess, $"ApproveReview failed: {approveResult.ErrorMessage}");
        Assert.Equal("Published", approveResult.Model!.Root.Status);

        // ── Step 4: Initialize aggregate rating for the product ─────────
        var emptyRatingModel = new AggregateRatingAnemicModel
        {
            Root = AggregateRatingRoot.CreateInstance(
                Guid.NewGuid(), productId, 0m, 0, 0, "Pending"),
            Distribution = RatingDistribution.Empty(),
            WeightedAverage = 0m
        };

        var ratingAggregate = AggregateRatingAggregate.CreateInstance(emptyRatingModel);
        var initResult = ratingAggregate.InitializeRating(productId);

        Assert.True(initResult.IsSuccess, $"InitializeRating failed: {initResult.ErrorMessage}");
        Assert.Equal("Pending", initResult.Model!.Root.Status);
        Assert.Equal(0, initResult.Model.Root.TotalReviews);

        // ── Step 5: Recalculate rating with the review data ─────────────
        // One 5-star verified review
        var ratingAggregateForRecalc = AggregateRatingAggregate.CreateInstance(initResult.Model);
        var recalcResult = ratingAggregateForRecalc.RecalculateRating(
            oneStar: 0,
            twoStar: 0,
            threeStar: 0,
            fourStar: 0,
            fiveStar: 1,
            verifiedReviews: 1,
            totalVerifiedRatingSum: 5,
            totalUnverifiedRatingSum: 0);

        Assert.True(recalcResult.IsSuccess, $"RecalculateRating failed: {recalcResult.ErrorMessage}");

        // ── Final assertions ────────────────────────────────────────────
        // Review is Published
        Assert.Equal("Published", approveResult.Model.Root.Status);

        // AggregateRating has correct values
        var finalRating = recalcResult.Model!;
        Assert.Equal(5.0m, finalRating.Root.AverageRating);
        Assert.Equal(1, finalRating.Root.TotalReviews);
        Assert.Equal(1, finalRating.Root.VerifiedReviews);
        Assert.Equal(0, finalRating.Distribution.OneStar);
        Assert.Equal(1, finalRating.Distribution.FiveStar);

        // Weighted average: verified counts double → (5*2)/(1*2) = 5.0
        Assert.Equal(5.0m, finalRating.WeightedAverage);

        // Status remains Pending (< 3 reviews needed for Active)
        Assert.Equal("Pending", finalRating.Root.Status);
    }
}
