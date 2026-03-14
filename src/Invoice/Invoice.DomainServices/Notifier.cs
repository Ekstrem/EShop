namespace Invoice.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using Invoice.Domain.Implementation;

public sealed class Notifier : IObservable<AggregateResult<IInvoice, IInvoiceAnemicModel>>
{
    private readonly List<IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>>> _observers = new();
    private Aggregate _aggregate;

    private Notifier(Aggregate aggregate)
    {
        _aggregate = aggregate;
    }

    public static Notifier CreateInstance(IInvoiceAnemicModel model)
        => new(Aggregate.CreateInstance(model));

    public static Notifier CreateEmpty()
        => new(Aggregate.CreateInstance(new AnemicModel()));

    public IDisposable Subscribe(IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>> observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> GenerateInvoice(
        string invoiceNumber, Guid orderId, Guid customerId, IReadOnlyList<IInvoiceLine> lines)
    {
        var result = _aggregate.GenerateInvoice(invoiceNumber, orderId, customerId, lines);
        Notify(result);
        return result;
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> SendInvoice()
    {
        var result = _aggregate.SendInvoice();
        Notify(result);
        return result;
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> ResendInvoice()
    {
        var result = _aggregate.ResendInvoice();
        Notify(result);
        return result;
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> GenerateCreditNote(
        string creditNoteNumber, decimal refundAmount)
    {
        var result = _aggregate.GenerateCreditNote(creditNoteNumber, refundAmount);
        Notify(result);
        return result;
    }

    private void Notify(AggregateResult<IInvoice, IInvoiceAnemicModel> result)
    {
        foreach (var observer in _observers)
            observer.OnNext(result);
    }

    private sealed class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>>> _observers;
        private readonly IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>> _observer;

        public Unsubscriber(
            List<IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>>> observers,
            IObserver<AggregateResult<IInvoice, IInvoiceAnemicModel>> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}
