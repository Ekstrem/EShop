using ReturnRequest.Domain;

namespace ReturnRequest.InternalContracts;

/// <summary>
/// Query repository contract for reading return request data.
/// </summary>
public interface IReturnRequestQueryRepository : IQueryRepository<IReturnRequest>
{
    Task<ReturnRequestReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReturnRequestReadModel>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReturnRequestReadModel>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReturnRequestReadModel>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<ReturnRequestReadModel?> GetByRmaNumberAsync(string rmaNumber, CancellationToken cancellationToken = default);
}
