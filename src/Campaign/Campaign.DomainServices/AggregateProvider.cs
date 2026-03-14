namespace Campaign.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Campaign.Domain;
using Campaign.Domain.Abstraction;

public sealed class AggregateProvider : IAggregateProvider<ICampaign, ICampaignAnemicModel>
{
    public IAggregate<ICampaign, ICampaignAnemicModel> Create(ICampaignAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
