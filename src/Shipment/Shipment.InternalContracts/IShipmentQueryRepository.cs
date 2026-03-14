using Shipment.Domain;
using Shipment.Domain.Abstraction;

namespace Shipment.InternalContracts;

/// <summary>
/// Query repository contract for reading shipment data.
/// </summary>
public interface IShipmentQueryRepository : IQueryRepository<IShipment>
{
    Task<ShipmentReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ShipmentReadModel?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ShipmentReadModel>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ShipmentReadModel>> GetByCarrierAsync(string carrier, CancellationToken cancellationToken = default);
}
