namespace Campaign.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Campaign.Domain;
using Campaign.Domain.Abstraction;

public sealed record CancelCampaignCommand(Guid CampaignId)
    : IRequest<AggregateResult<ICampaign, ICampaignAnemicModel>>;
