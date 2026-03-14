namespace Campaign.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Campaign.Domain;
using Campaign.Domain.Abstraction;

public sealed record CompleteSendingCommand(
    Guid CampaignId,
    int SentCount,
    int FailedCount)
    : IRequest<AggregateResult<ICampaign, ICampaignAnemicModel>>;
