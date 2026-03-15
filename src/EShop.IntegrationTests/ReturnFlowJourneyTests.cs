namespace EShop.IntegrationTests;

using Xunit;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;

// Shipment
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Specifications;
using ShipmentImpl = Shipment.Domain.Implementation;

// ReturnRequest
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.Domain.Specifications;
using ReturnRequestImpl = ReturnRequest.Domain.Implementation;

// StockItem
using StockItem.Domain;
using StockItem.Domain.Abstraction;
using StockItemImpl = StockItem.Domain.Implementation;

// Payment
using Payment.Domain;
using Payment.Domain.Abstraction;
using PaymentImpl = Payment.Domain.Implementation;

/// <summary>
/// Journey 7: Shipment -> ReturnRequest -> StockItem -> Payment
/// Return flow from a delivered shipment through return processing, stock replenishment, and refund.
/// </summary>
public class ReturnFlowJourneyTests
{
    private readonly Guid _customerId = Guid.NewGuid();
    private readonly Guid _orderId = Guid.NewGuid();
    private readonly Guid _variantId = Guid.NewGuid();
    private readonly Guid _warehouseId = Guid.NewGuid();

    [Fact]
    public void ReturnFlow_DeliveredShipmentToRefund_AllTransitionsSucceed()
    {
        // ---------------------------------------------------------------
        // Step 1: Start with a delivered shipment
        // ---------------------------------------------------------------
        var shipmentId = Guid.NewGuid();
        var deliveredShipmentRoot = ShipmentImpl.ShipmentRoot.CreateInstance(
            shipmentId, _orderId, "TRK-99999", "UPS",
            "789 Pine Rd, Capital City, 10001, US",
            "Delivered", DateTime.UtcNow.AddDays(-2));
        var shipmentItems = new List<IShipmentItem>
        {
            ShipmentImpl.ShipmentItem.CreateInstance(_variantId, "Premium Widget", 2)
        }.AsReadOnly();
        var shipmentLabel = ShipmentImpl.ShippingLabel.CreateInstance(
            "https://labels.example.com/label-999", DateTime.UtcNow.AddDays(-5));

        var deliveredShipment = new ShipmentImpl.AnemicModel
        {
            Root = deliveredShipmentRoot,
            Items = shipmentItems,
            Label = shipmentLabel
        };

        // Verify shipment is in Delivered status
        Assert.Equal("Delivered", deliveredShipment.Root.Status);
        var isNotDeliveredValidator = new IsNotDeliveredValidator();
        Assert.False(isNotDeliveredValidator.IsSatisfiedBy(deliveredShipment));

        // Cannot transition beyond Delivered
        var beyondDeliveredValidator = new SequentialStatusValidator("Delivered");
        Assert.False(beyondDeliveredValidator.IsSatisfiedBy(deliveredShipment));

        // ---------------------------------------------------------------
        // Step 2: Create return request (Requested status)
        // ---------------------------------------------------------------
        var returnId = Guid.NewGuid();
        var returnRoot = ReturnRequestImpl.ReturnRequestRoot.CreateInstance(
            returnId, _orderId, _customerId,
            "RMA-000001", "Item arrived damaged",
            "Requested", DateTime.UtcNow);
        var returnItems = new List<IReturnItem>
        {
            ReturnRequestImpl.ReturnItem.CreateInstance(_variantId, "Premium Widget", 2, 45.00m)
        }.AsReadOnly();

        var requestedReturn = new ReturnRequestImpl.AnemicModel
        {
            Root = returnRoot,
            Items = returnItems,
            ReturnLabel = null,
            RefundAmount = 90.00m // 2 x 45.00
        };

        // Validate Requested status
        var isRequestedValidator = new IsRequestedValidator();
        Assert.True(isRequestedValidator.IsSatisfiedBy(requestedReturn));

        // ---------------------------------------------------------------
        // Step 3: Approve return (Requested -> Approved)
        // ---------------------------------------------------------------
        var approvedRoot = ReturnRequestImpl.ReturnRequestRoot.CreateInstance(
            returnId, _orderId, _customerId,
            "RMA-000001", "Item arrived damaged",
            "Approved", DateTime.UtcNow);
        var returnLabel = ReturnRequestImpl.ReturnLabel.CreateInstance(
            "https://returns.example.com/label-rma001", "UPS");

        var approvedReturn = new ReturnRequestImpl.AnemicModel
        {
            Root = approvedRoot,
            Items = returnItems,
            ReturnLabel = returnLabel,
            RefundAmount = 90.00m
        };

        var isApprovedValidator = new IsApprovedValidator();
        Assert.True(isApprovedValidator.IsSatisfiedBy(approvedReturn));
        // The old status should no longer satisfy Requested
        Assert.False(isRequestedValidator.IsSatisfiedBy(approvedReturn));

        // ---------------------------------------------------------------
        // Step 4: Mark return as shipped (Approved -> ReturnShipped)
        // ---------------------------------------------------------------
        var returnShippedRoot = ReturnRequestImpl.ReturnRequestRoot.CreateInstance(
            returnId, _orderId, _customerId,
            "RMA-000001", "Item arrived damaged",
            "ReturnShipped", DateTime.UtcNow);

        var returnShippedModel = new ReturnRequestImpl.AnemicModel
        {
            Root = returnShippedRoot,
            Items = returnItems,
            ReturnLabel = returnLabel,
            RefundAmount = 90.00m
        };

        var isReturnShippedValidator = new IsReturnShippedValidator();
        Assert.True(isReturnShippedValidator.IsSatisfiedBy(returnShippedModel));
        Assert.False(isApprovedValidator.IsSatisfiedBy(returnShippedModel));

        // ---------------------------------------------------------------
        // Step 5: Receive return (ReturnShipped -> Received)
        // ---------------------------------------------------------------
        var receivedRoot = ReturnRequestImpl.ReturnRequestRoot.CreateInstance(
            returnId, _orderId, _customerId,
            "RMA-000001", "Item arrived damaged",
            "Received", DateTime.UtcNow);

        var receivedReturn = new ReturnRequestImpl.AnemicModel
        {
            Root = receivedRoot,
            Items = returnItems,
            ReturnLabel = returnLabel,
            RefundAmount = 90.00m
        };

        var isReceivedValidator = new IsReceivedValidator();
        Assert.True(isReceivedValidator.IsSatisfiedBy(receivedReturn));
        Assert.False(isReturnShippedValidator.IsSatisfiedBy(receivedReturn));

        // ---------------------------------------------------------------
        // Step 6: Replenish stock for returned items
        // ---------------------------------------------------------------
        // Start with stock that had items reserved and shipped out (total reduced)
        var stockRoot = StockItemImpl.StockItemRoot.CreateInstance(
            _variantId, _warehouseId, 48, 0, 10); // 50 original - 2 shipped
        var stockModel = new StockItemImpl.AnemicModel
        {
            Root = stockRoot,
            Reservations = new List<IReservation>()
        };

        var stockAggregate = StockItemImpl.Aggregate.CreateInstance(stockModel);
        var replenishResult = stockAggregate.ReplenishStock(2); // Return 2 items
        Assert.True(replenishResult.IsSuccess());
        Assert.Equal(50, replenishResult.Model()!.Root.Total); // Back to original

        // ---------------------------------------------------------------
        // Step 7: Issue refund on payment
        // ---------------------------------------------------------------
        // Start with a Completed payment for this order
        var paymentRoot = PaymentImpl.PaymentRoot.CreateInstance(
            _orderId, 95.00m, "USD", "CreditCard", "Completed");
        var chargeTransaction = PaymentImpl.Transaction.CreateInstance(
            "txn_original_charge", "Charge", 95.00m, "Completed",
            DateTime.UtcNow.AddDays(-5));
        var paymentModel = new PaymentImpl.AnemicModel
        {
            Root = paymentRoot,
            Transactions = new List<ITransaction> { chargeTransaction }
        };

        var paymentAggregate = PaymentImpl.Aggregate.CreateInstance(paymentModel);
        var refundResult = paymentAggregate.RequestRefund(90.00m); // Refund item cost only
        Assert.True(refundResult.IsSuccess());
        Assert.Equal("PartiallyRefunded", refundResult.Model()!.Root.Status);

        // Verify refund transaction was added
        var refundTransactions = refundResult.Model()!.Transactions
            .Where(t => t.Type == "Refund" && t.Status == "Completed")
            .ToList();
        Assert.Single(refundTransactions);
        Assert.Equal(90.00m, refundTransactions.First().Amount);

        // ---------------------------------------------------------------
        // Final assertions: all state transitions are valid
        // ---------------------------------------------------------------
        // Return request went through: Requested -> Approved -> ReturnShipped -> Received
        Assert.True(isReceivedValidator.IsSatisfiedBy(receivedReturn));

        // Stock was replenished
        Assert.Equal(50, replenishResult.Model()!.Root.Total);

        // Partial refund was issued (90 out of 95 - shipping not refunded)
        Assert.Equal("PartiallyRefunded", refundResult.Model()!.Root.Status);
    }

    [Fact]
    public void ReturnFlow_FullRefundReplenishesStockCompletely()
    {
        // Start with a Completed payment
        var paymentRoot = PaymentImpl.PaymentRoot.CreateInstance(
            _orderId, 90.00m, "USD", "CreditCard", "Completed");
        var chargeTransaction = PaymentImpl.Transaction.CreateInstance(
            "txn_charge_full", "Charge", 90.00m, "Completed",
            DateTime.UtcNow.AddDays(-3));
        var paymentModel = new PaymentImpl.AnemicModel
        {
            Root = paymentRoot,
            Transactions = new List<ITransaction> { chargeTransaction }
        };

        // Full refund
        var paymentAggregate = PaymentImpl.Aggregate.CreateInstance(paymentModel);
        var refundResult = paymentAggregate.RequestRefund(90.00m);
        Assert.True(refundResult.IsSuccess());
        Assert.Equal("FullyRefunded", refundResult.Model()!.Root.Status);

        // Replenish all returned stock
        var stockRoot = StockItemImpl.StockItemRoot.CreateInstance(
            _variantId, _warehouseId, 18, 0, 10); // Low stock after items shipped
        var stockModel = new StockItemImpl.AnemicModel
        {
            Root = stockRoot,
            Reservations = new List<IReservation>()
        };
        var stockAggregate = StockItemImpl.Aggregate.CreateInstance(stockModel);
        var replenishResult = stockAggregate.ReplenishStock(2);
        Assert.True(replenishResult.IsSuccess());
        Assert.Equal(20, replenishResult.Model()!.Root.Total);

        // Final assertions
        Assert.Equal("FullyRefunded", refundResult.Model()!.Root.Status);
        Assert.Equal(20, replenishResult.Model()!.Root.Total);
    }

    [Fact]
    public void ReturnFlow_InvalidStatusTransitions_AreRejectedByValidators()
    {
        var returnId = Guid.NewGuid();

        // Create a return in Requested status
        var requestedRoot = ReturnRequestImpl.ReturnRequestRoot.CreateInstance(
            returnId, _orderId, _customerId,
            "RMA-000002", "Wrong size",
            "Requested", DateTime.UtcNow);
        var returnItems = new List<IReturnItem>
        {
            ReturnRequestImpl.ReturnItem.CreateInstance(_variantId, "Shoes", 1, 120.00m)
        }.AsReadOnly();
        var requestedModel = new ReturnRequestImpl.AnemicModel
        {
            Root = requestedRoot,
            Items = returnItems,
            ReturnLabel = null,
            RefundAmount = 120.00m
        };

        // Requested status should NOT satisfy Approved, ReturnShipped, or Received
        var isApprovedValidator = new IsApprovedValidator();
        var isReturnShippedValidator = new IsReturnShippedValidator();
        var isReceivedValidator = new IsReceivedValidator();

        Assert.False(isApprovedValidator.IsSatisfiedBy(requestedModel));
        Assert.False(isReturnShippedValidator.IsSatisfiedBy(requestedModel));
        Assert.False(isReceivedValidator.IsSatisfiedBy(requestedModel));

        // Approved status should NOT satisfy Requested or Received
        var approvedRoot = ReturnRequestImpl.ReturnRequestRoot.CreateInstance(
            returnId, _orderId, _customerId,
            "RMA-000002", "Wrong size",
            "Approved", DateTime.UtcNow);
        var approvedModel = new ReturnRequestImpl.AnemicModel
        {
            Root = approvedRoot,
            Items = returnItems,
            ReturnLabel = null,
            RefundAmount = 120.00m
        };

        var isRequestedValidator = new IsRequestedValidator();
        Assert.False(isRequestedValidator.IsSatisfiedBy(approvedModel));
        Assert.False(isReceivedValidator.IsSatisfiedBy(approvedModel));
        Assert.True(isApprovedValidator.IsSatisfiedBy(approvedModel));
    }
}
