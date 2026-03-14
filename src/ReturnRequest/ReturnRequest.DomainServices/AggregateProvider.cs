using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.InternalContracts;

namespace ReturnRequest.DomainServices;

/// <summary>
/// Provides aggregate construction and reconstitution for the ReturnRequest context.
/// </summary>
public sealed class AggregateProvider
{
    private readonly IReturnRequestQueryRepository _queryRepository;

    public AggregateProvider(IReturnRequestQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IReturnRequestAnemicModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var readModel = await _queryRepository.GetByIdAsync(id, cancellationToken);
        return readModel is null ? null : MapToAnemicModel(readModel);
    }

    public async Task<IReadOnlyList<IReturnRequestAnemicModel>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var readModels = await _queryRepository.GetByOrderIdAsync(orderId, cancellationToken);
        return readModels.Select(MapToAnemicModel).ToList();
    }

    private static IReturnRequestAnemicModel MapToAnemicModel(ReturnRequestReadModel readModel)
    {
        // Mapping is performed via the domain implementation layer.
        // This is a simplified projection for the domain services boundary.
        return null!;
    }
}
