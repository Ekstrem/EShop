namespace Campaign.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Campaign.Domain;
using Campaign.Domain.Abstraction;

public sealed record UpdateCampaignCommand(
    Guid CampaignId,
    string Name,
    string Subject,
    string TemplateId,
    string SegmentId)
    : IRequest<AggregateResult<ICampaign, ICampaignAnemicModel>>;
