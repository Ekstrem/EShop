namespace EShop.IntegrationTests;

using Xunit;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;

// Order
using Order.Domain;
using Order.Domain.Abstraction;
using Order.Domain.Aggregate;
using OrderImpl = Order.Domain.Implementation;

// StockItem
using StockItem.Domain;
using StockItem.Domain.Abstraction;
using StockItemImpl = StockItem.Domain.Implementation;

// Payment
using Payment.Domain;
using Payment.Domain.Abstraction;
using PaymentImpl = Payment.Domain.Implementation;

/// <summary>
/// Journey 5-6: Order cancellation flows.
/// Tests cancellation of both Placed and Paid orders with stock release and payment handling.
/// </summary>
public class OrderCancellationJourneyTests
{
    private readonly Guid _customerId = Guid.NewGuid();
    private readonly Guid _variantId = Guid.NewGuid();
    private readonly Guid _warehouseId = Guid.NewGuid();

    private IOrderAnemicModel CreateOrderInPlacedState(Guid orderId)
    {
        var orderRoot = OrderImpl.OrderRoot.CreateInstance(
            _customerId,
            "ORD-000010",
            "Placed",
            DateTime.UtcNow,
            "456 Oak Ave, Shelbyville, 62705, US");

        var orderLines = new List<IOrderLine>
        {
            OrderImpl.OrderLine.CreateInstance(
                _variantId, "Gadget X", "SKU-100", 3, 30.00m, 0m)
        };

        var orderTotal = OrderImpl.OrderTotal.CreateInstance(90.00m, 0m, 5.00m, 95.00m);

        return new OrderImpl.AnemicModel
        {
            Root = orderRoot,
            Lines = orderLines.AsReadOnly(),
            OrderTotal = orderTotal
        };
    }

    private (StockItemImpl.Aggregate aggregate, AggregateResult<IStockItem, IStockItemAnemicModel> result) ReserveStockForOrder(Guid orderId, int quantity)
    {
        var stockRoot = StockItemImpl.StockItemRoot.CreateInstance(
            _variantId, _warehouseId, 100, 0, 10);
        var stockModel = new StockItemImpl.AnemicModel
        {
            Root = stockRoot,
            Reservations = new List<IReservation>()
        };
        var aggregate = StockItemImpl.Aggregate.CreateInstance(stockModel);
        var result = aggregate.ReserveStock(orderId, quantity);
        return (aggregate, result);
    }

    /// <summary>
    /// Journey 5: Placed order cancellation.
    /// 1. Create an order in Placed state
    /// 2. Reserve stock
    /// 3. Cancel order (Placed -> Cancelled)
    /// 4. Release reserved stock
    /// 5. Void payment (if initiated)
    /// </summary>
    [Fact]
    public void PlacedOrderCancellation_CancelsOrderAndReleasesStock()
    {
        // Step 1: Create order in Placed state
        var orderId = Guid.NewGuid();
        var orderModel = CreateOrderInPlacedState(orderId);
        Assert.Equal("Placed", orderModel.Root.Status);

        // Step 2: Reserve stock
        var (_, reserveResult) = ReserveStockForOrder(orderId, 3);
        Assert.True(reserveResult.IsSuccess);
        Assert.Equal(3, reserveResult.Model!.Root.Reserved);
        Assert.Single(reserveResult.Model!.Reservations);

        // Step 3: Cancel order (Placed -> Cancelled)
        var cancelResult = OrderAggregate.CancelOrder(orderModel);
        Assert.True(cancelResult.IsSuccess);
        Assert.Equal("Cancelled", cancelResult.Model!.Root.Status);
        Assert.Equal("OrderCancelled", cancelResult.EventName);

        // Step 4: Release reserved stock
        var stockAggregate = StockItemImpl.Aggregate.CreateInstance(reserveResult.Model!);
        var releaseResult = stockAggregate.ReleaseStock(orderId);
        Assert.True(releaseResult.IsSuccess);
        Assert.Equal(0, releaseResult.Model!.Root.Reserved);
        Assert.Empty(releaseResult.Model!.Reservations);

        // Step 5: Void payment (if initiated)
        var paymentRoot = PaymentImpl.PaymentRoot.CreateInstance(
            Guid.Empty, 0m, "USD", "CreditCard");
        var emptyPaymentModel = new PaymentImpl.AnemicModel
        {
            Root = paymentRoot,
            Transactions = new List<ITransaction>()
        };
        var paymentAggregate = PaymentImpl.Aggregate.CreateInstance(emptyPaymentModel);
        var initiateResult = paymentAggregate.InitiatePayment(
            orderId, 95.00m, "USD", "CreditCard");
        Assert.True(initiateResult.IsSuccess);
        Assert.Equal("Initiated", initiateResult.Model!.Root.Status);

        var paymentAggregate2 = PaymentImpl.Aggregate.CreateInstance(initiateResult.Model!);
        var voidResult = paymentAggregate2.VoidPayment();
        Assert.True(voidResult.IsSuccess);
        Assert.Equal("Voided", voidResult.Model!.Root.Status);

        // Final assertions
        Assert.Equal("Cancelled", cancelResult.Model!.Root.Status);
        Assert.Equal(0, releaseResult.Model!.Root.Reserved);
        Assert.Equal("Voided", voidResult.Model!.Root.Status);
    }

    /// <summary>
    /// Journey 6: Paid order cancellation.
    /// 1. Create an order, complete payment (Paid state)
    /// 2. Cancel order (Paid -> Cancelled)
    /// 3. Release reserved stock
    /// 4. Request refund on payment
    /// </summary>
    [Fact]
    public void PaidOrderCancellation_CancelsOrderReleasesStockAndRefunds()
    {
        var orderId = Guid.NewGuid();

        // Step 1a: Create order in Placed state
        var orderModel = CreateOrderInPlacedState(orderId);
        Assert.Equal("Placed", orderModel.Root.Status);

        // Step 1b: Reserve stock
        var (_, reserveResult) = ReserveStockForOrder(orderId, 3);
        Assert.True(reserveResult.IsSuccess);

        // Step 1c: Initiate and complete payment
        var paymentRoot = PaymentImpl.PaymentRoot.CreateInstance(
            Guid.Empty, 0m, "USD", "CreditCard");
        var emptyPaymentModel = new PaymentImpl.AnemicModel
        {
            Root = paymentRoot,
            Transactions = new List<ITransaction>()
        };
        var paymentAggregate = PaymentImpl.Aggregate.CreateInstance(emptyPaymentModel);
        var initiateResult = paymentAggregate.InitiatePayment(
            orderId, 95.00m, "USD", "CreditCard");
        Assert.True(initiateResult.IsSuccess);

        var paymentAggregate2 = PaymentImpl.Aggregate.CreateInstance(initiateResult.Model!);
        var webhookResult = paymentAggregate2.HandleProviderWebhook(
            "txn_" + Guid.NewGuid().ToString("N"),
            "Charge",
            95.00m,
            "Completed");
        Assert.True(webhookResult.IsSuccess);
        Assert.Equal("Completed", webhookResult.Model!.Root.Status);

        // Step 1d: Confirm payment on order (Placed -> Paid)
        var confirmPaymentResult = OrderAggregate.ConfirmPayment(orderModel);
        Assert.True(confirmPaymentResult.IsSuccess);
        Assert.Equal("Paid", confirmPaymentResult.Model!.Root.Status);

        var paidOrderModel = confirmPaymentResult.Model!;

        // Step 2: Cancel order (Paid -> Cancelled)
        var cancelResult = OrderAggregate.CancelOrder(paidOrderModel);
        Assert.True(cancelResult.IsSuccess);
        Assert.Equal("Cancelled", cancelResult.Model!.Root.Status);
        Assert.Equal("OrderCancelled", cancelResult.EventName);

        // Step 3: Release reserved stock
        var stockAggregate = StockItemImpl.Aggregate.CreateInstance(reserveResult.Model!);
        var releaseResult = stockAggregate.ReleaseStock(orderId);
        Assert.True(releaseResult.IsSuccess);
        Assert.Equal(0, releaseResult.Model!.Root.Reserved);
        Assert.Empty(releaseResult.Model!.Reservations);

        // Step 4: Request refund on payment
        var paymentAggregate3 = PaymentImpl.Aggregate.CreateInstance(webhookResult.Model!);
        var refundResult = paymentAggregate3.RequestRefund(95.00m);
        Assert.True(refundResult.IsSuccess);
        Assert.Equal("FullyRefunded", refundResult.Model!.Root.Status);

        // Final assertions
        Assert.Equal("Cancelled", cancelResult.Model!.Root.Status);
        Assert.Equal(0, releaseResult.Model!.Root.Reserved);
        Assert.Equal("FullyRefunded", refundResult.Model!.Root.Status);
    }
}
