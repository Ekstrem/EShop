namespace Campaign.DomainServices;

using Campaign.Domain.Abstraction;

public sealed class AggregateProvider
{
    public object Create(ICampaignAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
