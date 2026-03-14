namespace Invoice.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.InternalContracts;

public sealed class AggregateProvider
{
    private readonly IQueryRepository<IInvoice, IInvoiceAnemicModel> _repository;

    public AggregateProvider(IQueryRepository<IInvoice, IInvoiceAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<Notifier> LoadAsync(Guid id, CancellationToken ct = default)
    {
        var model = await _repository.GetByIdAsync(id, ct);
        return Notifier.CreateInstance(model);
    }

    public Notifier CreateNew()
    {
        return Notifier.CreateEmpty();
    }
}
