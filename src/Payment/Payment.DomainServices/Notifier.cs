namespace Payment.DomainServices;

using DigiTFactory.Libraries.SeedWorks.Events;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Payment.Domain;
using Payment.Domain.Abstraction;
using Payment.Domain.Implementation;

public sealed class Notifier : INotifier<IPayment>, IObservable<AggregateResult<IPayment, IPaymentAnemicModel>>
{
    private readonly List<IObserver<AggregateResult<IPayment, IPaymentAnemicModel>>> _observers = new();
    private Aggregate _aggregate;

    private Notifier(Aggregate aggregate)
    {
        _aggregate = aggregate;
    }

    public static Notifier CreateInstance(IPaymentAnemicModel model)
        => new(Aggregate.CreateInstance(model));

    public static Notifier CreateEmpty()
        => new(Aggregate.CreateInstance(new AnemicModel()));

    public IDisposable Subscribe(IObserver<AggregateResult<IPayment, IPaymentAnemicModel>> observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> InitiatePayment(
        Guid orderId, decimal amount, string currency, string paymentMethod)
    {
        var result = _aggregate.InitiatePayment(orderId, amount, currency, paymentMethod);
        Notify(result);
        return result;
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> HandleProviderWebhook(
        string providerTransactionId, string transactionType, decimal amount, string transactionStatus)
    {
        var result = _aggregate.HandleProviderWebhook(
            providerTransactionId, transactionType, amount, transactionStatus);
        Notify(result);
        return result;
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> VoidPayment()
    {
        var result = _aggregate.VoidPayment();
        Notify(result);
        return result;
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> RequestRefund(decimal refundAmount)
    {
        var result = _aggregate.RequestRefund(refundAmount);
        Notify(result);
        return result;
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> CapturePayment(string providerTransactionId)
    {
        var result = _aggregate.CapturePayment(providerTransactionId);
        Notify(result);
        return result;
    }

    void INotifier<IPayment>.Notify<TModel>(AggregateResult<IPayment, TModel> result)
    {
    }

    private void Notify(AggregateResult<IPayment, IPaymentAnemicModel> result)
    {
        foreach (var observer in _observers)
            observer.OnNext(result);
    }

    private sealed class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<AggregateResult<IPayment, IPaymentAnemicModel>>> _observers;
        private readonly IObserver<AggregateResult<IPayment, IPaymentAnemicModel>> _observer;

        public Unsubscriber(
            List<IObserver<AggregateResult<IPayment, IPaymentAnemicModel>>> observers,
            IObserver<AggregateResult<IPayment, IPaymentAnemicModel>> observer)
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
