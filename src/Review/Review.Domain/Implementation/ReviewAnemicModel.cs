using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Review.Domain.Abstraction;

namespace Review.Domain.Implementation;

/// <summary>
/// Immutable anemic model for the Review aggregate.
/// </summary>
public sealed class ReviewAnemicModel : IReviewAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IReviewRoot Root { get; set; } = null!;
    public IReadOnlyList<IHelpfulVote> HelpfulVotes { get; set; } = new List<IHelpfulVote>();
    public IReadOnlyList<IFlag> Flags { get; set; } = new List<IFlag>();
    public int HelpfulCount => HelpfulVotes.Count;

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
