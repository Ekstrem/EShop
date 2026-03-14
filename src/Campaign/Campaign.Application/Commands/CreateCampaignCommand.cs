namespace Campaign.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Campaign.Domain;
using Campaign.Domain.Abstraction;

public sealed record CreateCampaignCommand(
    string Name,
    string Subject,
    string TemplateId,
    string SegmentId)
    : IRequest<AggregateResult<ICampaign, ICampaignAnemicModel>>;
