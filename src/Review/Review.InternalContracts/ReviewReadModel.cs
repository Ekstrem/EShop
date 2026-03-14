namespace Review.InternalContracts;

public sealed class ReviewReadModel
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public Guid CustomerId { get; init; }
    public int Rating { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public bool IsVerifiedPurchase { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string? ModeratorResponse { get; init; }
    public int HelpfulCount { get; init; }
    public IReadOnlyList<HelpfulVoteReadModel> HelpfulVotes { get; init; } = new List<HelpfulVoteReadModel>();
    public IReadOnlyList<FlagReadModel> Flags { get; init; } = new List<FlagReadModel>();
}

public sealed class HelpfulVoteReadModel
{
    public Guid VoterId { get; init; }
    public DateTime VotedAt { get; init; }
}

public sealed class FlagReadModel
{
    public Guid FlaggerId { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime FlaggedAt { get; init; }
}
