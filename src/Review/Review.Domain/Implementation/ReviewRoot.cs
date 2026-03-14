using Review.Domain.Abstraction;

namespace Review.Domain.Implementation;

/// <summary>
/// Immutable implementation of the Review aggregate root.
/// </summary>
public sealed class ReviewRoot : IReviewRoot
{
    private ReviewRoot(
        Guid id,
        Guid productId,
        Guid customerId,
        int rating,
        string title,
        string text,
        bool isVerifiedPurchase,
        string status,
        DateTime createdAt,
        string? moderatorResponse)
    {
        Id = id;
        ProductId = productId;
        CustomerId = customerId;
        Rating = rating;
        Title = title;
        Text = text;
        IsVerifiedPurchase = isVerifiedPurchase;
        Status = status;
        CreatedAt = createdAt;
        ModeratorResponse = moderatorResponse;
    }

    public Guid Id { get; }
    public Guid ProductId { get; }
    public Guid CustomerId { get; }
    public int Rating { get; }
    public string Title { get; }
    public string Text { get; }
    public bool IsVerifiedPurchase { get; }
    public string Status { get; }
    public DateTime CreatedAt { get; }
    public string? ModeratorResponse { get; }

    public static ReviewRoot CreateInstance(
        Guid id,
        Guid productId,
        Guid customerId,
        int rating,
        string title,
        string text,
        bool isVerifiedPurchase,
        string status,
        DateTime createdAt,
        string? moderatorResponse = null)
    {
        return new ReviewRoot(
            id, productId, customerId, rating, title, text,
            isVerifiedPurchase, status, createdAt, moderatorResponse);
    }
}
