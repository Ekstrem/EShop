namespace Campaign.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Campaign.Domain;
using Campaign.Domain.Abstraction;

public sealed record ScheduleCampaignCommand(
    Guid CampaignId,
    DateTime ScheduledAt)
    : IRequest<AggregateResult<ICampaign, ICampaignAnemicModel>>;
