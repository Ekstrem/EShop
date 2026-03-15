namespace EShop.IntegrationTests;

using Xunit;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;

// Cart
using Cart.Domain;
using Cart.Domain.Abstraction;
using Cart.Domain.Implementation;
using Cart.Domain.Aggregate;

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

// Invoice
using Invoice.Domain;
using Invoice.Domain.Abstraction;
using InvoiceImpl = Invoice.Domain.Implementation;

// Shipment
using Shipment.Domain;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Specifications;
using ShipmentImpl = Shipment.Domain.Implementation;

/// <summary>
/// Journey 4: Cart -> Order -> StockItem -> Payment -> Invoice -> Shipment
/// Full purchase happy path from adding items to a cart through to delivery.
/// </summary>
public class PurchaseHappyPathJourneyTests
{
    private readonly Guid _customerId = Guid.NewGuid();
    private readonly Guid _sessionId = Guid.NewGuid();
    private readonly Guid _variantId1 = Guid.NewGuid();
    private readonly Guid _variantId2 = Guid.NewGuid();
    private readonly Guid _warehouseId = Guid.NewGuid();

    [Fact]
    public void FullPurchaseJourney_CartToDelivery_AllTransitionsSucceed()
    {
        // ---------------------------------------------------------------
        // Step 1: Create a cart and add items
        // ---------------------------------------------------------------
        var cartRoot = CartRoot.CreateInstance(_customerId, _sessionId);
        var shippingAddress = ShippingAddress.CreateInstance(
            "123 Main St", "Springfield", "62704", "US");

        var initialCartModel = new Cart.Domain.Implementation.AnemicModel
        {
            Root = cartRoot,
            Items = new List<ICartItem>().AsReadOnly(),
            AppliedPromoCode = null,
            ShippingAddress = shippingAddress
        };

        // Add first item
        var addItem1Result = CartAggregate.AddItemToCart(
            initialCartModel, _variantId1, "Widget A", "SKU-001", 2, 25.00m);
        Assert.True(addItem1Result.IsSuccess());

        // Add second item
        var addItem2Result = CartAggregate.AddItemToCart(
            addItem1Result.Model()!, _variantId2, "Widget B", "SKU-002", 1, 50.00m);
        Assert.True(addItem2Result.IsSuccess());
        Assert.Equal(2, addItem2Result.Model()!.Items.Count);

        // Step 1b: Place order (transition cart to CheckedOut)
        var placeOrderResult = CartAggregate.PlaceOrder(addItem2Result.Model()!);
        Assert.True(placeOrderResult.IsSuccess());
        Assert.Equal("CheckedOut", placeOrderResult.Model()!.Root.Status);

        // ---------------------------------------------------------------
        // Step 2: Create an order from cart data (Placed status)
        // ---------------------------------------------------------------
        var cartModel = placeOrderResult.Model()!;
        var orderLines = cartModel.Items.Select(item =>
            OrderImpl.OrderLine.CreateInstance(
                item.VariantId, item.ProductName, item.Sku,
                item.Quantity, item.UnitPrice, 0m)).ToList();

        var subTotal = cartModel.Items.Sum(i => i.Quantity * i.UnitPrice);
        var orderTotal = OrderImpl.OrderTotal.CreateInstance(subTotal, 0m, 5.00m, subTotal + 5.00m);

        var orderRoot = OrderImpl.OrderRoot.CreateInstance(
            _customerId,
            "ORD-000001",
            "Placed",
            DateTime.UtcNow,
            "123 Main St, Springfield, 62704, US");

        var orderModel = new OrderImpl.AnemicModel
        {
            Root = orderRoot,
            Lines = orderLines.Cast<IOrderLine>().ToList().AsReadOnly(),
            OrderTotal = orderTotal
        };

        Assert.Equal("Placed", orderModel.Root.Status);
        Assert.Equal(2, orderModel.Lines.Count);

        // ---------------------------------------------------------------
        // Step 3: Reserve stock for order items
        // ---------------------------------------------------------------
        var stockRoot1 = StockItemImpl.StockItemRoot.CreateInstance(
            _variantId1, _warehouseId, 100, 0, 10);
        var stockModel1 = new StockItemImpl.AnemicModel
        {
            Root = stockRoot1,
            Reservations = new List<IReservation>()
        };
        var stockAggregate1 = StockItemImpl.Aggregate.CreateInstance(stockModel1);
        var reserveResult1 = stockAggregate1.ReserveStock(Guid.NewGuid(), 2);
        Assert.True(reserveResult1.IsSuccess());
        Assert.Equal(2, reserveResult1.Model()!.Root.Reserved);

        var stockRoot2 = StockItemImpl.StockItemRoot.CreateInstance(
            _variantId2, _warehouseId, 50, 0, 5);
        var stockModel2 = new StockItemImpl.AnemicModel
        {
            Root = stockRoot2,
            Reservations = new List<IReservation>()
        };
        var stockAggregate2 = StockItemImpl.Aggregate.CreateInstance(stockModel2);
        var reserveResult2 = stockAggregate2.ReserveStock(Guid.NewGuid(), 1);
        Assert.True(reserveResult2.IsSuccess());
        Assert.Equal(1, reserveResult2.Model()!.Root.Reserved);

        // ---------------------------------------------------------------
        // Step 4: Initiate payment for the order
        // ---------------------------------------------------------------
        var orderId = Guid.NewGuid();
        var emptyPaymentRoot = PaymentImpl.PaymentRoot.CreateInstance(
            Guid.Empty, 0m, "USD", "CreditCard");
        var emptyPaymentModel = new PaymentImpl.AnemicModel
        {
            Root = emptyPaymentRoot,
            Transactions = new List<ITransaction>()
        };
        var paymentAggregate = PaymentImpl.Aggregate.CreateInstance(emptyPaymentModel);
        var initiateResult = paymentAggregate.InitiatePayment(
            orderId, orderTotal.Total, "USD", "CreditCard");
        Assert.True(initiateResult.IsSuccess());
        Assert.Equal("Initiated", initiateResult.Model()!.Root.Status);

        // ---------------------------------------------------------------
        // Step 5: Complete payment (HandleProviderWebhook with success)
        // ---------------------------------------------------------------
        var paymentAggregate2 = PaymentImpl.Aggregate.CreateInstance(initiateResult.Model()!);
        var webhookResult = paymentAggregate2.HandleProviderWebhook(
            "txn_" + Guid.NewGuid().ToString("N"),
            "Charge",
            orderTotal.Total,
            "Completed");
        Assert.True(webhookResult.IsSuccess());
        Assert.Equal("Completed", webhookResult.Model()!.Root.Status);

        // ---------------------------------------------------------------
        // Step 6: Confirm payment on order (Placed -> Paid)
        // ---------------------------------------------------------------
        var confirmPaymentResult = OrderAggregate.ConfirmPayment(orderModel);
        Assert.True(confirmPaymentResult.IsSuccess());
        Assert.Equal("Paid", confirmPaymentResult.Model()!.Root.Status);
        // EventName not available on AggregateResult

        var paidOrderModel = confirmPaymentResult.Model()!;

        // ---------------------------------------------------------------
        // Step 7: Generate invoice for the paid order
        // ---------------------------------------------------------------
        var invoiceLines = paidOrderModel.Lines.Select(line =>
            InvoiceImpl.InvoiceLine.CreateInstance(
                line.ProductName,
                line.Quantity,
                line.UnitPrice,
                21.0m) // 21% VAT
        ).Cast<IInvoiceLine>().ToList().AsReadOnly();

        var emptyInvoiceRoot = InvoiceImpl.InvoiceRoot.CreateInstance(
            "INV-000000", Guid.Empty, Guid.Empty, DateTime.UtcNow);
        var emptyInvoiceModel = new InvoiceImpl.AnemicModel
        {
            Root = emptyInvoiceRoot,
            Lines = new List<IInvoiceLine>(),
            Totals = InvoiceImpl.InvoiceTotals.CreateInstance(0, 0, 0)
        };
        var invoiceAggregate = InvoiceImpl.Aggregate.CreateInstance(emptyInvoiceModel);
        var generateResult = invoiceAggregate.GenerateInvoice(
            "INV-000001", orderId, _customerId, invoiceLines);
        Assert.True(generateResult.IsSuccess());
        Assert.Equal("Generated", generateResult.Model()!.Root.Status);
        Assert.Equal("Invoice", generateResult.Model()!.Root.InvoiceType);

        // ---------------------------------------------------------------
        // Step 8: Send invoice
        // ---------------------------------------------------------------
        var invoiceAggregate2 = InvoiceImpl.Aggregate.CreateInstance(generateResult.Model()!);
        var sendResult = invoiceAggregate2.SendInvoice();
        Assert.True(sendResult.IsSuccess());
        Assert.Equal("Sent", sendResult.Model()!.Root.Status);

        // ---------------------------------------------------------------
        // Step 9: Create shipment, pack, dispatch
        // (Shipment has no aggregate; use specs-based validation)
        // ---------------------------------------------------------------
        var shipmentId = Guid.NewGuid();

        // 9a: Create shipment in Pending status
        var shipmentRoot = ShipmentImpl.ShipmentRoot.CreateInstance(
            shipmentId, orderId, "TRK-12345", "FedEx",
            "123 Main St, Springfield, 62704, US",
            "Pending", DateTime.UtcNow);
        var shipmentItems = paidOrderModel.Lines.Select(line =>
            ShipmentImpl.ShipmentItem.CreateInstance(
                line.VariantId, line.ProductName, line.Quantity)
        ).Cast<IShipmentItem>().ToList().AsReadOnly();
        var shipmentLabel = ShipmentImpl.ShippingLabel.CreateInstance(
            "https://labels.example.com/label123", DateTime.UtcNow);

        var pendingShipment = new ShipmentImpl.AnemicModel
        {
            Root = shipmentRoot,
            Items = shipmentItems,
            Label = shipmentLabel
        };

        // Validate Pending status
        var isPendingValidator = new IsPendingValidator();
        Assert.True(isPendingValidator.IsSatisfiedBy(pendingShipment));

        // 9b: Pack (Pending -> Packed)
        var packedTransitionValidator = new SequentialStatusValidator("Packed");
        Assert.True(packedTransitionValidator.IsSatisfiedBy(pendingShipment));

        var packedShipmentRoot = ShipmentImpl.ShipmentRoot.CreateInstance(
            shipmentId, orderId, "TRK-12345", "FedEx",
            "123 Main St, Springfield, 62704, US",
            "Packed", DateTime.UtcNow);
        var packedShipment = new ShipmentImpl.AnemicModel
        {
            Root = packedShipmentRoot,
            Items = shipmentItems,
            Label = shipmentLabel
        };
        var isPackedValidator = new IsPackedValidator();
        Assert.True(isPackedValidator.IsSatisfiedBy(packedShipment));

        // 9c: Dispatch (Packed -> Shipped)
        var shippedTransitionValidator = new SequentialStatusValidator("Shipped");
        Assert.True(shippedTransitionValidator.IsSatisfiedBy(packedShipment));

        var shippedShipmentRoot = ShipmentImpl.ShipmentRoot.CreateInstance(
            shipmentId, orderId, "TRK-12345", "FedEx",
            "123 Main St, Springfield, 62704, US",
            "Shipped", DateTime.UtcNow);
        var shippedShipment = new ShipmentImpl.AnemicModel
        {
            Root = shippedShipmentRoot,
            Items = shipmentItems,
            Label = shipmentLabel
        };

        // Validate tracking number is present
        var hasTrackingValidator = new HasTrackingNumberValidator();
        Assert.True(hasTrackingValidator.IsSatisfiedBy(shippedShipment));

        // Confirm shipment on order (Paid -> Shipped)
        var confirmShipmentResult = OrderAggregate.ConfirmShipment(paidOrderModel);
        Assert.True(confirmShipmentResult.IsSuccess());
        Assert.Equal("Shipped", confirmShipmentResult.Model()!.Root.Status);

        // ---------------------------------------------------------------
        // Step 10: Confirm delivery
        // ---------------------------------------------------------------
        // Shipment: Shipped -> InTransit -> Delivered
        var inTransitValidator = new SequentialStatusValidator("InTransit");
        Assert.True(inTransitValidator.IsSatisfiedBy(shippedShipment));

        var inTransitRoot = ShipmentImpl.ShipmentRoot.CreateInstance(
            shipmentId, orderId, "TRK-12345", "FedEx",
            "123 Main St, Springfield, 62704, US",
            "InTransit", DateTime.UtcNow);
        var inTransitShipment = new ShipmentImpl.AnemicModel
        {
            Root = inTransitRoot,
            Items = shipmentItems,
            Label = shipmentLabel
        };

        var deliveredValidator = new SequentialStatusValidator("Delivered");
        Assert.True(deliveredValidator.IsSatisfiedBy(inTransitShipment));

        var deliveredRoot = ShipmentImpl.ShipmentRoot.CreateInstance(
            shipmentId, orderId, "TRK-12345", "FedEx",
            "123 Main St, Springfield, 62704, US",
            "Delivered", DateTime.UtcNow);
        var deliveredShipment = new ShipmentImpl.AnemicModel
        {
            Root = deliveredRoot,
            Items = shipmentItems,
            Label = shipmentLabel
        };

        // Cannot go beyond Delivered
        var isNotDeliveredValidator = new IsNotDeliveredValidator();
        Assert.False(isNotDeliveredValidator.IsSatisfiedBy(deliveredShipment));

        // Confirm delivery on order (Shipped -> Delivered)
        var shippedOrderModel = confirmShipmentResult.Model()!;
        var confirmDeliveryResult = OrderAggregate.ConfirmDelivery(shippedOrderModel);
        Assert.True(confirmDeliveryResult.IsSuccess());
        Assert.Equal("Delivered", confirmDeliveryResult.Model()!.Root.Status);
        // EventName not available on AggregateResult

        // ---------------------------------------------------------------
        // Final assertions
        // ---------------------------------------------------------------
        Assert.Equal("Delivered", confirmDeliveryResult.Model()!.Root.Status);
        Assert.Equal("Completed", webhookResult.Model()!.Root.Status);
        Assert.Equal("Sent", sendResult.Model()!.Root.Status);
        Assert.Equal("Delivered", deliveredShipment.Root.Status);
    }
}
