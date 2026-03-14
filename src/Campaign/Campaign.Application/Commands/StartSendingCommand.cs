namespace Campaign.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Campaign.Domain;
using Campaign.Domain.Abstraction;

public sealed record StartSendingCommand(
    Guid CampaignId,
    int TotalRecipients)
    : IRequest<AggregateResult<ICampaign, ICampaignAnemicModel>>;
