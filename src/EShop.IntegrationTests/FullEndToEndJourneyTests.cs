using EShop.Contracts;
using Xunit;
using Category.Domain.Abstraction;
using Category.Domain.Implementation;
using Product.Domain.Abstraction;
using Product.Domain.Implementation;
using StockItem.Domain.Abstraction;
using StockItem.Domain.Implementation;
using Customer.Domain.Abstraction;
using Customer.Domain.Implementation;
using Session.Domain.Abstraction;
using Session.Domain.Implementation;
using Cart.Domain.Abstraction;
using Cart.Domain.Implementation;
using Cart.Domain.Aggregate;
using Order.Domain.Abstraction;
using Order.Domain.Implementation;
using Order.Domain.Aggregate;
using Payment.Domain.Abstraction;
using Payment.Domain.Implementation;
using Invoice.Domain.Abstraction;
using Invoice.Domain.Implementation;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Implementation;
using Review.Domain.Abstraction;
using Review.Domain.Implementation;
using AggregateRating.Domain.Abstraction;
using AggregateRating.Domain.Implementation;
using Notification.Domain.Abstraction;
using Notification.Domain.Implementation;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;

namespace EShop.IntegrationTests;

/// <summary>
/// Full end-to-end journey spanning all 17 bounded contexts:
/// Category → Product → StockItem → Customer → Session → Cart →
/// DiscountCode → Promotion → Order → Payment → Invoice → Shipment →
/// Review → AggregateRating → Campaign → Notification
/// </summary>
public sealed class FullEndToEndJourneyTests
{
    [Fact]
    public void FullECommerceJourney_AllContextsParticipate_AllOperationsSucceed()
    {
        // Track context participation
        var participatedContexts = new HashSet<string>();

        // ── 1. Category: Create category hierarchy ──────────────────────
        var catModel = new Category.Domain.Implementation.AnemicModel();
        var catAggregate = Category.Domain.Implementation.Aggregate.CreateInstance(catModel);

        var parentCatResult = catAggregate.CreateCategory(
            "Electronics", parentId: null, depth: 0, sortOrder: 1,
            siblingNames: new List<string>());
        Assert.True(parentCatResult.IsSuccess(), $"CreateCategory (parent) failed: {parentCatResult.ErrorMessage()}");
        Assert.Equal("Active", parentCatResult.Model()!.Root.Status);

        var childCatAggregate = Category.Domain.Implementation.Aggregate.CreateInstance(
            new Category.Domain.Implementation.AnemicModel());
        var childCatResult = childCatAggregate.CreateCategory(
            "Smartphones", parentId: Guid.NewGuid(), depth: 1, sortOrder: 1,
            siblingNames: new List<string>());
        Assert.True(childCatResult.IsSuccess(), $"CreateCategory (child) failed: {childCatResult.ErrorMessage()}");
        participatedContexts.Add("Category");

        var categoryId = Guid.NewGuid(); // simulated persisted ID

        // ── 2. Product: Create and publish product ──────────────────────
        var prodModel = new Product.Domain.Implementation.AnemicModel();
        var prodAggregate = Product.Domain.Implementation.Aggregate.CreateInstance(prodModel);

        var variant = ProductVariant.CreateInstance("SKU-PHONE-001", "Standard", "Black", 999.99m);
        var media = ProductMedia.CreateInstance("https://img.example.com/phone.jpg", "Phone image", 1);

        var createProdResult = prodAggregate.CreateProduct(
            "Galaxy Phone Pro",
            "A premium smartphone with top-tier camera and performance",
            categoryId,
            new List<IProductVariant> { variant },
            new List<IProductMedia> { media });
        Assert.True(createProdResult.IsSuccess(), $"CreateProduct failed: {createProdResult.ErrorMessage()}");
        Assert.Equal("Draft", createProdResult.Model()!.Root.Status);

        var draftProdAggregate = Product.Domain.Implementation.Aggregate.CreateInstance(createProdResult.Model());
        var publishResult = draftProdAggregate.PublishProduct();
        Assert.True(publishResult.IsSuccess(), $"PublishProduct failed: {publishResult.ErrorMessage()}");
        Assert.Equal("Published", publishResult.Model()!.Root.Status);
        participatedContexts.Add("Product");

        var productId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        // ── 3. StockItem: Initialize stock ──────────────────────────────
        var stockRoot = StockItemRoot.CreateInstance(
            variantId, warehouseId: Guid.NewGuid(), total: 50, reserved: 0, lowStockThreshold: 5);
        var stockModel = new StockItem.Domain.Implementation.AnemicModel
        {
            Root = stockRoot,
            Reservations = new List<IReservation>()
        };
        Assert.Equal("InStock", stockModel.Root.Status);
        participatedContexts.Add("StockItem");

        // ── 4. Customer: Register and verify ────────────────────────────
        var registerResult = CustomerAggregate.RegisterCustomer(
            "john.doe@example.com", "John", "Doe", "hashed-password-123");
        Assert.True(registerResult.IsSuccess(), $"RegisterCustomer failed: {registerResult.ErrorMessage()}");
        Assert.Equal("Unverified", registerResult.Model()!.Root.Status);

        var verifyResult = CustomerAggregate.VerifyEmail(registerResult.Model());
        Assert.True(verifyResult.IsSuccess(), $"VerifyEmail failed: {verifyResult.ErrorMessage()}");
        Assert.Equal("Active", verifyResult.Model()!.Root.Status);
        participatedContexts.Add("Customer");

        var customerId = verifyResult.Model().Root.Id;

        // ── 5. Session: Create session ──────────────────────────────────
        var sessionResult = SessionAggregate.CreateSession(
            customerId, "jwt-token-abc123", TimeSpan.FromHours(2), "Chrome/Windows");
        Assert.True(sessionResult.IsSuccess(), $"CreateSession failed: {sessionResult.ErrorMessage()}");
        Assert.Equal("Active", sessionResult.Model()!.Root.Status);
        participatedContexts.Add("Session");

        var sessionId = sessionResult.Model().Root.Id;

        // ── 6. Cart: Add to cart, apply discount ────────────────────────
        var cartRoot = CartRoot.CreateInstance(customerId, sessionId);
        var cartModel = new Cart.Domain.Implementation.AnemicModel
        {
            Root = cartRoot,
            Items = new List<ICartItem>(),
            AppliedPromoCode = null,
            ShippingAddress = null
        };

        var addItemResult = CartAggregate.AddItemToCart(
            cartModel, variantId, "Galaxy Phone Pro", "SKU-PHONE-001", 1, 999.99m);
        Assert.True(addItemResult.IsSuccess(), $"AddItemToCart failed: {addItemResult.ErrorMessage()}");
        Assert.Single(addItemResult.Model()!.Items);
        participatedContexts.Add("Cart");

        // ── 6b. DiscountCode + Promotion: Apply promo ───────────────────
        // Create promotion
        var promoModel = new Promotion.Domain.Implementation.AnemicModel();
        var promoAggregate = Promotion.Domain.Implementation.Aggregate.CreateInstance(promoModel);
        var createPromoResult = promoAggregate.CreatePromotion(
            "Launch Discount", "10% off phones", "Percentage", 10m,
            DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(30),
            "Smartphones only", false);
        Assert.True(createPromoResult.IsSuccess(), $"CreatePromotion failed: {createPromoResult.ErrorMessage()}");

        var activatePromoAggregate = Promotion.Domain.Implementation.Aggregate.CreateInstance(createPromoResult.Model()!);
        var activatePromoResult = activatePromoAggregate.ActivatePromotion();
        Assert.True(activatePromoResult.IsSuccess(), $"ActivatePromotion failed: {activatePromoResult.ErrorMessage()}");
        participatedContexts.Add("Promotion");

        // Generate discount code
        var codeModel = new DiscountCode.Domain.Implementation.AnemicModel();
        var codeAggregate = DiscountCode.Domain.Implementation.Aggregate.CreateInstance(codeModel);
        var genCodeResult = codeAggregate.GenerateDiscountCode(
            "LAUNCH10", Guid.NewGuid(), maxUsage: 1000, expiresAt: DateTime.UtcNow.AddDays(30));
        Assert.True(genCodeResult.IsSuccess(), $"GenerateDiscountCode failed: {genCodeResult.ErrorMessage()}");
        participatedContexts.Add("DiscountCode");

        // Apply promo to cart
        var applyPromoResult = CartAggregate.ApplyPromoCode(
            addItemResult.Model(), "LAUNCH10", discountPercent: 10m, discountAmount: 100m);
        Assert.True(applyPromoResult.IsSuccess(), $"ApplyPromoCode failed: {applyPromoResult.ErrorMessage()}");
        Assert.NotNull(applyPromoResult.Model()!.AppliedPromoCode);

        // ── 7. Order: Place order ───────────────────────────────────────
        var placeOrderCartResult = CartAggregate.PlaceOrder(applyPromoResult.Model());
        Assert.True(placeOrderCartResult.IsSuccess(), $"PlaceOrder (Cart) failed: {placeOrderCartResult.ErrorMessage()}");
        Assert.Equal("CheckedOut", placeOrderCartResult.Model()!.Root.Status);

        // Build Order model from cart data
        var orderId = Guid.NewGuid();
        var orderRoot = OrderRoot.CreateInstance(
            customerId, "ORD-20260315-001", "Placed", DateTime.UtcNow,
            "123 Main St, Springfield, IL 62701");
        var orderLine = OrderLine.CreateInstance(
            variantId, "Galaxy Phone Pro", "SKU-PHONE-001", 1, 999.99m, 100m);
        var orderTotal = OrderTotal.CreateInstance(999.99m, 100m, 9.99m, 909.98m);
        var orderModel = new Order.Domain.Implementation.AnemicModel
        {
            Root = orderRoot,
            Lines = new List<IOrderLine> { orderLine },
            OrderTotal = orderTotal
        };
        Assert.Equal("Placed", orderModel.Root.Status);
        participatedContexts.Add("Order");

        // ── 8. StockItem: Reserve stock ─────────────────────────────────
        var stockAggregate = StockItem.Domain.Implementation.Aggregate.CreateInstance(stockModel);
        var reserveResult = stockAggregate.ReserveStock(orderId, quantity: 1);
        Assert.True(reserveResult.IsSuccess(), $"ReserveStock failed: {reserveResult.ErrorMessage()}");
        Assert.Equal(1, reserveResult.Model()!.Root.Reserved);

        // ── 8b. Payment: Initiate and complete payment ──────────────────
        var payModel = new Payment.Domain.Implementation.AnemicModel();
        var payAggregate = Payment.Domain.Implementation.Aggregate.CreateInstance(payModel);
        var initiateResult = payAggregate.InitiatePayment(
            orderId, 909.98m, "USD", "CreditCard");
        Assert.True(initiateResult.IsSuccess(), $"InitiatePayment failed: {initiateResult.ErrorMessage()}");
        Assert.Equal("Initiated", initiateResult.Model()!.Root.Status);

        var initiatedPayAggregate = Payment.Domain.Implementation.Aggregate.CreateInstance(initiateResult.Model());
        var captureResult = initiatedPayAggregate.CapturePayment("txn-stripe-12345");
        Assert.True(captureResult.IsSuccess(), $"CapturePayment failed: {captureResult.ErrorMessage()}");
        Assert.Equal("Completed", captureResult.Model()!.Root.Status);
        participatedContexts.Add("Payment");

        // Confirm payment on order
        var confirmPayResult = OrderAggregate.ConfirmPayment(orderModel);
        Assert.True(confirmPayResult.IsSuccess(), $"ConfirmPayment failed: {confirmPayResult.ErrorMessage()}");
        Assert.Equal("Paid", confirmPayResult.Model()!.Root.Status);

        // ── 9. Invoice: Generate and send invoice ───────────────────────
        var invModel = new Invoice.Domain.Implementation.AnemicModel();
        var invAggregate = Invoice.Domain.Implementation.Aggregate.CreateInstance(invModel);
        var invoiceLine = InvoiceLine.CreateInstance("Galaxy Phone Pro x1", 1, 899.99m, 10m);
        var genInvResult = invAggregate.GenerateInvoice(
            "INV-000001", orderId, customerId,
            new List<IInvoiceLine> { invoiceLine });
        Assert.True(genInvResult.IsSuccess(), $"GenerateInvoice failed: {genInvResult.ErrorMessage()}");
        Assert.Equal("Generated", genInvResult.Model()!.Root.Status);

        var genInvAggregate = Invoice.Domain.Implementation.Aggregate.CreateInstance(genInvResult.Model());
        var sendInvResult = genInvAggregate.SendInvoice();
        Assert.True(sendInvResult.IsSuccess(), $"SendInvoice failed: {sendInvResult.ErrorMessage()}");
        Assert.Equal("Sent", sendInvResult.Model()!.Root.Status);
        participatedContexts.Add("Invoice");

        // ── 10. Shipment: Create and deliver shipment ───────────────────
        var shipRoot = ShipmentRoot.CreateInstance(
            Guid.NewGuid(), orderId, "TRACK-SHIP-001", "UPS",
            "123 Main St, Springfield, IL 62701", "Delivered", DateTime.UtcNow);
        var shipItem = ShipmentItem.CreateInstance(variantId, "Galaxy Phone Pro", 1);
        var shipLabel = ShippingLabel.CreateInstance(
            "https://labels.example.com/ship001.pdf", DateTime.UtcNow.AddHours(-2));
        var shipmentModel = new Shipment.Domain.Implementation.AnemicModel
        {
            Root = shipRoot,
            Items = new List<IShipmentItem> { shipItem },
            Label = shipLabel
        };
        Assert.Equal("Delivered", shipmentModel.Root.Status);
        participatedContexts.Add("Shipment");

        // Confirm delivery on order
        var deliveryResult = OrderAggregate.ConfirmDelivery(confirmPayResult.Model());
        Assert.True(deliveryResult.IsSuccess(), $"ConfirmDelivery failed: {deliveryResult.ErrorMessage()}");
        Assert.Equal("Delivered", deliveryResult.Model()!.Root.Status);

        // ── 11. Review: Submit and approve review ───────────────────────
        var emptyReviewModel = new ReviewAnemicModel();
        var reviewAggregate = ReviewAggregate.CreateInstance(emptyReviewModel);
        var submitResult = reviewAggregate.SubmitReview(
            productId, customerId, 5, "Amazing phone!",
            "The best phone I have ever used. Camera is incredible and the battery lasts all day.",
            isVerifiedPurchase: true,
            reviewExistsCheck: (_, _) => false);
        Assert.True(submitResult.IsSuccess(), $"SubmitReview failed: {submitResult.ErrorMessage()}");

        var submittedReviewAggregate = ReviewAggregate.CreateInstance(submitResult.Model()!);
        var approveResult = submittedReviewAggregate.ApproveReview();
        Assert.True(approveResult.IsSuccess(), $"ApproveReview failed: {approveResult.ErrorMessage()}");
        Assert.Equal("Published", approveResult.Model()!.Root.Status);
        participatedContexts.Add("Review");

        // ── 12. AggregateRating: Calculate aggregate rating ─────────────
        var emptyRatingModel = new AggregateRatingAnemicModel
        {
            Root = AggregateRatingRoot.CreateInstance(
                Guid.NewGuid(), productId, 0m, 0, 0, "Pending"),
            Distribution = RatingDistribution.Empty(),
            WeightedAverage = 0m
        };
        var ratingAggregate = AggregateRatingAggregate.CreateInstance(emptyRatingModel);
        var initRatingResult = ratingAggregate.InitializeRating(productId);
        Assert.True(initRatingResult.IsSuccess(), $"InitializeRating failed: {initRatingResult.ErrorMessage()}");

        var ratingForRecalc = AggregateRatingAggregate.CreateInstance(initRatingResult.Model()!);
        var recalcResult = ratingForRecalc.RecalculateRating(
            oneStar: 0, twoStar: 0, threeStar: 0, fourStar: 0, fiveStar: 1,
            verifiedReviews: 1, totalVerifiedRatingSum: 5, totalUnverifiedRatingSum: 0);
        Assert.True(recalcResult.IsSuccess(), $"RecalculateRating failed: {recalcResult.ErrorMessage()}");
        Assert.Equal(5.0m, recalcResult.Model()!.Root.AverageRating);
        participatedContexts.Add("AggregateRating");

        // ── 13. Campaign: Create a campaign ─────────────────────────────
        var campaignModel = new Campaign.Domain.Implementation.AnemicModel();
        var campaignAggregate = Campaign.Domain.Implementation.Aggregate.CreateInstance(campaignModel);
        var createCampResult = campaignAggregate.CreateCampaign(
            "New Phone Launch", "Check out the Galaxy Phone Pro!",
            "tmpl-phone-launch", "seg-phone-enthusiasts");
        Assert.True(createCampResult.IsSuccess(), $"CreateCampaign failed: {createCampResult.ErrorMessage()}");
        Assert.Equal("Draft", createCampResult.Model()!.Root.Status);
        participatedContexts.Add("Campaign");

        // ── 14. Notification: Create notification ───────────────────────
        var notifModel = new Notification.Domain.Implementation.AnemicModel();
        var notifAggregate = Notification.Domain.Implementation.Aggregate.CreateInstance(notifModel);
        var createNotifResult = notifAggregate.CreateNotification(
            customerId, Guid.NewGuid(), "Email", "tmpl-order-delivered", "en-US", "Transactional");
        Assert.True(createNotifResult.IsSuccess(), $"CreateNotification failed: {createNotifResult.ErrorMessage()}");

        var notifCreatedAggregate = Notification.Domain.Implementation.Aggregate.CreateInstance(createNotifResult.Model()!);
        var renderResult = notifCreatedAggregate.Render(
            "<html><body>Your Galaxy Phone Pro has been delivered!</body></html>",
            "Your order has been delivered");
        Assert.True(renderResult.IsSuccess(), $"Render notification failed: {renderResult.ErrorMessage()}");

        var notifRenderedAggregate = Notification.Domain.Implementation.Aggregate.CreateInstance(renderResult.Model()!);
        var sendNotifResult = notifRenderedAggregate.Send(hasConsent: true);
        Assert.True(sendNotifResult.IsSuccess(), $"Send notification failed: {sendNotifResult.ErrorMessage()}");
        Assert.Equal("Sent", sendNotifResult.Model()!.Root.Status);
        participatedContexts.Add("Notification");

        // ── Final assertions: All contexts participated ─────────────────
        var expectedContexts = new[]
        {
            "Category", "Product", "StockItem", "Customer", "Session",
            "Cart", "Promotion", "DiscountCode", "Order", "Payment",
            "Invoice", "Shipment", "Review", "AggregateRating",
            "Campaign", "Notification"
        };

        foreach (var ctx in expectedContexts)
        {
            Assert.Contains(ctx, participatedContexts);
        }

        // Verify final states across all contexts
        Assert.Equal("Active", parentCatResult.Model()!.Root.Status);      // Category
        Assert.Equal("Published", publishResult.Model()!.Root.Status);     // Product
        Assert.Equal(1, reserveResult.Model()!.Root.Reserved);             // StockItem
        Assert.Equal("Active", verifyResult.Model()!.Root.Status);         // Customer
        Assert.Equal("Active", sessionResult.Model()!.Root.Status);        // Session
        Assert.Equal("CheckedOut", placeOrderCartResult.Model()!.Root.Status); // Cart
        Assert.Equal("Active", activatePromoResult.Model()!.Root.Status);  // Promotion
        Assert.Equal("Active", genCodeResult.Model()!.Root.Status);        // DiscountCode
        Assert.Equal("Delivered", deliveryResult.Model()!.Root.Status);    // Order
        Assert.Equal("Completed", captureResult.Model()!.Root.Status);     // Payment
        Assert.Equal("Sent", sendInvResult.Model()!.Root.Status);          // Invoice
        Assert.Equal("Delivered", shipmentModel.Root.Status);            // Shipment
        Assert.Equal("Published", approveResult.Model()!.Root.Status);     // Review
        Assert.Equal(5.0m, recalcResult.Model()!.Root.AverageRating);      // AggregateRating
        Assert.Equal("Draft", createCampResult.Model()!.Root.Status);      // Campaign
        Assert.Equal("Sent", sendNotifResult.Model()!.Root.Status);        // Notification
    }
}
