namespace Campaign.Application.Queries;

using MediatR;
using Campaign.InternalContracts;

public sealed class GetCampaignHandler : IRequestHandler<GetCampaignQuery, CampaignReadModel?>
{
    private readonly ICampaignQueryRepository _repository;

    public GetCampaignHandler(ICampaignQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CampaignReadModel?> Handle(
        GetCampaignQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.CampaignId, cancellationToken);
    }
}
