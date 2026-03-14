using Review.Domain.Abstraction;

namespace Review.Domain.Implementation;

/// <summary>
/// Immutable implementation of a flag value object.
/// </summary>
public sealed class Flag : IFlag
{
    private Flag(Guid flaggerId, string reason, DateTime flaggedAt)
    {
        FlaggerId = flaggerId;
        Reason = reason;
        FlaggedAt = flaggedAt;
    }

    public Guid FlaggerId { get; }
    public string Reason { get; }
    public DateTime FlaggedAt { get; }

    public static Flag CreateInstance(Guid flaggerId, string reason, DateTime flaggedAt)
    {
        return new Flag(flaggerId, reason, flaggedAt);
    }
}
