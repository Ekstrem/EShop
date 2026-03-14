namespace Campaign.InternalContracts;

public interface ICampaignQueryRepository
{
    Task<CampaignReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CampaignReadModel>> SearchAsync(
        string? name,
        string? status,
        int skip,
        int take,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(
        string? name,
        string? status,
        CancellationToken cancellationToken = default);
}
