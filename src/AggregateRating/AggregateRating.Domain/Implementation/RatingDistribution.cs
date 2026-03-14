using AggregateRating.Domain.Abstraction;

namespace AggregateRating.Domain.Implementation;

/// <summary>
/// Immutable implementation of a rating distribution value object.
/// </summary>
public sealed class RatingDistribution : IRatingDistribution
{
    private RatingDistribution(
        int oneStar,
        int twoStar,
        int threeStar,
        int fourStar,
        int fiveStar)
    {
        OneStar = oneStar;
        TwoStar = twoStar;
        ThreeStar = threeStar;
        FourStar = fourStar;
        FiveStar = fiveStar;
    }

    public int OneStar { get; }
    public int TwoStar { get; }
    public int ThreeStar { get; }
    public int FourStar { get; }
    public int FiveStar { get; }

    public static RatingDistribution CreateInstance(
        int oneStar,
        int twoStar,
        int threeStar,
        int fourStar,
        int fiveStar)
    {
        return new RatingDistribution(oneStar, twoStar, threeStar, fourStar, fiveStar);
    }

    public static RatingDistribution Empty()
    {
        return new RatingDistribution(0, 0, 0, 0, 0);
    }
}
