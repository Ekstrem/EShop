namespace Campaign.Application.Queries;

using MediatR;
using Campaign.InternalContracts;

public sealed record SearchCampaignsQuery(
    string? Name,
    string? Status,
    int Skip,
    int Take)
    : IRequest<IReadOnlyList<CampaignReadModel>>;
