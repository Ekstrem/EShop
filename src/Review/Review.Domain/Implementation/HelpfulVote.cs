using Review.Domain.Abstraction;

namespace Review.Domain.Implementation;

/// <summary>
/// Immutable implementation of a helpful vote value object.
/// </summary>
public sealed class HelpfulVote : IHelpfulVote
{
    private HelpfulVote(Guid voterId, DateTime votedAt)
    {
        VoterId = voterId;
        VotedAt = votedAt;
    }

    public Guid VoterId { get; }
    public DateTime VotedAt { get; }

    public static HelpfulVote CreateInstance(Guid voterId, DateTime votedAt)
    {
        return new HelpfulVote(voterId, votedAt);
    }
}
