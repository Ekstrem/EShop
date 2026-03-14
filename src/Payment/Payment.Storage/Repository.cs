namespace Payment.Storage;

using Microsoft.EntityFrameworkCore;
using Payment.Domain;
using Payment.Domain.Abstraction;
using Payment.Domain.Implementation;
using Payment.InternalContracts;

public sealed class Repository : IQueryRepository<IPayment, IPaymentAnemicModel>
{
    private readonly ReadDbContext _readContext;

    public Repository(ReadDbContext readContext)
    {
        _readContext = readContext;
    }

    public async Task<IPaymentAnemicModel> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _readContext.Payments
            .Include(p => p.Transactions)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

        if (entity is null)
            return new AnemicModel();

        return MapToAnemicModel(entity);
    }

    public async Task<IPaymentAnemicModel?> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default)
    {
        var entity = await _readContext.Payments
            .Include(p => p.Transactions)
            .FirstOrDefaultAsync(p => p.OrderId == orderId, ct);

        if (entity is null)
            return null;

        return MapToAnemicModel(entity);
    }

    public async Task<IReadOnlyList<IPaymentAnemicModel>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _readContext.Payments
            .Include(p => p.Transactions)
            .ToListAsync(ct);

        return entities.Select(MapToAnemicModel).ToList();
    }

    private static IPaymentAnemicModel MapToAnemicModel(PaymentEntity entity)
    {
        var root = PaymentRoot.CreateInstance(
            entity.OrderId,
            entity.Amount,
            entity.Currency,
            entity.PaymentMethod,
            entity.Status);

        var transactions = entity.Transactions.Select(t =>
            Transaction.CreateInstance(
                t.ProviderTransactionId,
                t.Type,
                t.Amount,
                t.Status,
                t.CreatedAt)).ToList();

        return new AnemicModel
        {
            Root = root,
            Transactions = transactions
        };
    }
}
