namespace Payment.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Payment.Domain.Abstraction;
using Payment.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public IPaymentAnemicModel Model { get; }

    private Aggregate(IPaymentAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(IPaymentAnemicModel model) => new(model);

    private AggregateResult<IPayment, IPaymentAnemicModel> Success(IPaymentAnemicModel newModel)
    {
        var data = BusinessOperationData<IPayment, IPaymentAnemicModel>
            .Commit<IPayment, IPaymentAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<IPayment, IPaymentAnemicModel>(data);
    }

    private AggregateResult<IPayment, IPaymentAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<IPayment, IPaymentAnemicModel>
            .Commit<IPayment, IPaymentAnemicModel>(Model, Model);
        return new AggregateResultException<IPayment, IPaymentAnemicModel>(
            data, new FailedSpecification<IPayment, IPaymentAnemicModel>(error));
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> InitiatePayment(
        Guid orderId,
        decimal amount,
        string currency,
        string paymentMethod)
    {
        var validator = new IdempotencyValidator();
        if (!validator.IsSatisfiedBy(Model, orderId))
            return Fail("A payment for this order already exists.");

        var root = PaymentRoot.CreateInstance(orderId, amount, currency, paymentMethod);
        var anemic = new AnemicModel
        {
            Root = root,
            Transactions = new List<ITransaction>()
        };

        return Success(anemic);
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> HandleProviderWebhook(
        string providerTransactionId,
        string transactionType,
        decimal amount,
        string transactionStatus)
    {
        var dedupValidator = new TransactionDeduplicationValidator();
        if (!dedupValidator.IsSatisfiedBy(Model, providerTransactionId))
            return Fail("Transaction with this provider ID has already been processed.");

        var transaction = Transaction.CreateInstance(
            providerTransactionId,
            transactionType,
            amount,
            transactionStatus,
            DateTime.UtcNow);

        var transactions = Model.Transactions.ToList();
        transactions.Add(transaction);

        var newStatus = DetermineStatusAfterWebhook(transactionType, transactionStatus, transactions);

        var root = PaymentRoot.CreateInstance(
            Model.Root.OrderId,
            Model.Root.Amount,
            Model.Root.Currency,
            Model.Root.PaymentMethod,
            newStatus);

        var anemic = new AnemicModel
        {
            Root = root,
            Transactions = transactions
        };

        return Success(anemic);
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> VoidPayment()
    {
        var validator = new CanVoidValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Only payments in Initiated status can be voided.");

        var root = PaymentRoot.CreateInstance(
            Model.Root.OrderId,
            Model.Root.Amount,
            Model.Root.Currency,
            Model.Root.PaymentMethod,
            "Voided");

        var anemic = new AnemicModel
        {
            Root = root,
            Transactions = Model.Transactions.ToList()
        };

        return Success(anemic);
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> RequestRefund(decimal refundAmount)
    {
        var statusValidator = new IsCompletedOrPartiallyRefundedValidator();
        if (!statusValidator.IsSatisfiedBy(Model))
            return Fail("Refunds can only be requested for completed or partially refunded payments.");

        var amountValidator = new RefundAmountValidator();
        if (!amountValidator.IsSatisfiedBy(Model, refundAmount))
            return Fail("Refund amount exceeds the remaining refundable amount.");

        var transaction = Transaction.CreateInstance(
            Guid.NewGuid().ToString(),
            "Refund",
            refundAmount,
            "Completed",
            DateTime.UtcNow);

        var transactions = Model.Transactions.ToList();
        transactions.Add(transaction);

        var totalRefunded = transactions
            .Where(t => t.Type == "Refund" && t.Status == "Completed")
            .Sum(t => t.Amount);

        var newStatus = totalRefunded >= Model.Root.Amount
            ? "FullyRefunded"
            : "PartiallyRefunded";

        var root = PaymentRoot.CreateInstance(
            Model.Root.OrderId,
            Model.Root.Amount,
            Model.Root.Currency,
            Model.Root.PaymentMethod,
            newStatus);

        var anemic = new AnemicModel
        {
            Root = root,
            Transactions = transactions
        };

        return Success(anemic);
    }

    public AggregateResult<IPayment, IPaymentAnemicModel> CapturePayment(
        string providerTransactionId)
    {
        var validator = new IsInitiatedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Only payments in Initiated status can be captured.");

        var transaction = Transaction.CreateInstance(
            providerTransactionId,
            "Charge",
            Model.Root.Amount,
            "Completed",
            DateTime.UtcNow);

        var transactions = Model.Transactions.ToList();
        transactions.Add(transaction);

        var root = PaymentRoot.CreateInstance(
            Model.Root.OrderId,
            Model.Root.Amount,
            Model.Root.Currency,
            Model.Root.PaymentMethod,
            "Completed");

        var anemic = new AnemicModel
        {
            Root = root,
            Transactions = transactions
        };

        return Success(anemic);
    }

    private static string DetermineStatusAfterWebhook(
        string transactionType,
        string transactionStatus,
        IReadOnlyList<ITransaction> transactions)
    {
        if (transactionType == "Charge" && transactionStatus == "Completed")
            return "Completed";
        if (transactionType == "Charge" && transactionStatus == "Failed")
            return "Failed";
        if (transactionType == "Void")
            return "Voided";
        if (transactionType == "Refund" && transactionStatus == "Completed")
        {
            var totalRefunded = transactions
                .Where(t => t.Type == "Refund" && t.Status == "Completed")
                .Sum(t => t.Amount);

            var chargeAmount = transactions
                .Where(t => t.Type == "Charge" && t.Status == "Completed")
                .Sum(t => t.Amount);

            return totalRefunded >= chargeAmount ? "FullyRefunded" : "PartiallyRefunded";
        }

        return "Initiated";
    }
}
