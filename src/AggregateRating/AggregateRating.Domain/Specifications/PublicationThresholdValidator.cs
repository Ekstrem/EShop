namespace AggregateRating.Domain.Specifications;

using AggregateRating.Domain.Abstraction;

internal sealed class PublicationThresholdValidator
{
    public bool IsSatisfiedBy(IAggregateRatingAnemicModel model)
        => model.Root.TotalReviews >= 3;

    public string Reason => "At least 3 published reviews are required for Active status.";
}
