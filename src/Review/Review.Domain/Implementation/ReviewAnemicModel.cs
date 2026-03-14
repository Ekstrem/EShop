using Hive.SeedWorks.TacticalPatterns;
using Review.Domain.Abstraction;

namespace Review.Domain.Implementation;

/// <summary>
/// Immutable anemic model for the Review aggregate.
/// </summary>
public sealed class ReviewAnemicModel : AnemicModel<IReview>, IReviewAnemicModel
{
    public IReviewRoot Root { get; set; } = null!;
    public IReadOnlyList<IHelpfulVote> HelpfulVotes { get; set; } = new List<IHelpfulVote>();
    public IReadOnlyList<IFlag> Flags { get; set; } = new List<IFlag>();
    public int HelpfulCount => HelpfulVotes.Count;
}
