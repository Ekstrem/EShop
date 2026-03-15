namespace Campaign.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Campaign.Domain;
using Campaign.Domain.Abstraction;

public sealed record ScheduleCampaignCommand(
    Guid CampaignId,
    DateTime ScheduledAt)
    : IRequest<AggregateResult<ICampaign, ICampaignAnemicModel>>;
