using Hive.SeedWorks.TacticalPatterns;

namespace Review.Domain.Abstraction;

/// <summary>
/// Anemic model contract for the Review aggregate.
/// </summary>
public interface IReviewAnemicModel : IAnemicModel<IReview>
{
    IReviewRoot Root { get; }
    IReadOnlyList<IHelpfulVote> HelpfulVotes { get; }
    IReadOnlyList<IFlag> Flags { get; }
    int HelpfulCount { get; }
}
