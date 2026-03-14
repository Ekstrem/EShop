namespace Payment.InternalContracts;

using Payment.Domain;
using Payment.Domain.Abstraction;

public interface IQueryRepository<TBC, TModel>
    where TBC : class
    where TModel : class
{
    Task<TModel> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<TModel?> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken ct = default);
}
