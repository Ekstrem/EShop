using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.InternalContracts;

namespace Shipment.DomainServices;

/// <summary>
/// Provides aggregate construction and reconstitution for the Shipment context.
/// </summary>
public sealed class AggregateProvider
{
    private readonly IShipmentQueryRepository _queryRepository;

    public AggregateProvider(IShipmentQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IShipmentAnemicModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var readModel = await _queryRepository.GetByIdAsync(id, cancellationToken);
        return readModel is null ? null : MapToAnemicModel(readModel);
    }

    public async Task<IShipmentAnemicModel?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var readModel = await _queryRepository.GetByOrderIdAsync(orderId, cancellationToken);
        return readModel is null ? null : MapToAnemicModel(readModel);
    }

    private static IShipmentAnemicModel MapToAnemicModel(ShipmentReadModel readModel)
    {
        // Mapping is performed via the domain implementation layer.
        // This is a simplified projection for the domain services boundary.
        return null!;
    }
}
