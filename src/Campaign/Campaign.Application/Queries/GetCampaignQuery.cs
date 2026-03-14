namespace Campaign.Application.Queries;

using MediatR;
using Campaign.InternalContracts;

public sealed record GetCampaignQuery(Guid CampaignId)
    : IRequest<CampaignReadModel?>;
