using Xunit;
using Notification.Domain;
using Notification.Domain.Abstraction;
using Notification.Domain.Implementation;
using INotification = Notification.Domain.INotification;

namespace EShop.IntegrationTests;

/// <summary>
/// Journey 10: Order/Payment/Shipment → Notification
/// Verifies transactional notification flows triggered by key domain events,
/// including retry logic on failure.
/// </summary>
public sealed class NotificationJourneyTests
{
    /// <summary>
    /// Creates a notification aggregate, renders, and sends it (transactional type).
    /// Returns the final send result.
    /// </summary>
    private static Hive.SeedWorks.Result.AggregateResult<INotification, INotificationAnemicModel>
        CreateRenderAndSend(Guid customerId, Guid eventId, string channel, string templateId)
    {
        // Create
        var emptyModel = new AnemicModel();
        var aggregate = Aggregate.CreateInstance(emptyModel);
        var createResult = aggregate.CreateNotification(
            customerId, eventId, channel, templateId, "en-US", "Transactional");

        if (!createResult.IsSuccess)
            return createResult;

        // Render
        var createdAggregate = Aggregate.CreateInstance(createResult.Model!);
        var renderResult = createdAggregate.Render(
            renderedContent: $"<html><body>Notification for event {eventId}</body></html>",
            subject: "Your order update");

        if (!renderResult.IsSuccess)
            return renderResult;

        // Send (transactional notifications do not require consent)
        var renderedAggregate = Aggregate.CreateInstance(renderResult.Model!);
        return renderedAggregate.Send(hasConsent: true);
    }

    [Fact]
    public void OrderPlaced_CreateTransactionalNotification_RenderAndSend_Succeeds()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var orderPlacedEventId = Guid.NewGuid();

        // Act — simulate OrderPlaced → notification
        var sendResult = CreateRenderAndSend(
            customerId, orderPlacedEventId, "Email", "tmpl-order-placed");

        // Assert
        Assert.True(sendResult.IsSuccess, $"OrderPlaced notification failed: {sendResult.ErrorMessage}");
        Assert.Equal("Sent", sendResult.Model!.Root.Status);
        Assert.Equal("Transactional", sendResult.Model.Root.Type);
        Assert.Equal(customerId, sendResult.Model.Root.CustomerId);
    }

    [Fact]
    public void PaymentCompleted_CreateTransactionalNotification_RenderAndSend_Succeeds()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var paymentCompletedEventId = Guid.NewGuid();

        // Act — simulate PaymentCompleted → notification
        var sendResult = CreateRenderAndSend(
            customerId, paymentCompletedEventId, "Email", "tmpl-payment-completed");

        // Assert
        Assert.True(sendResult.IsSuccess, $"PaymentCompleted notification failed: {sendResult.ErrorMessage}");
        Assert.Equal("Sent", sendResult.Model!.Root.Status);
        Assert.Equal("Transactional", sendResult.Model.Root.Type);
    }

    [Fact]
    public void MarketingNotification_FailedThenRetry_ReachesSentStatus()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var campaignEventId = Guid.NewGuid();

        // Step 1: Create a marketing notification
        var emptyModel = new AnemicModel();
        var aggregate = Aggregate.CreateInstance(emptyModel);
        var createResult = aggregate.CreateNotification(
            customerId, campaignEventId, "Email", "tmpl-campaign-promo", "en-US", "Marketing");

        Assert.True(createResult.IsSuccess, $"CreateNotification failed: {createResult.ErrorMessage}");
        Assert.Equal("Created", createResult.Model!.Root.Status);

        // Step 2: Render the notification
        var createdAggregate = Aggregate.CreateInstance(createResult.Model);
        var renderResult = createdAggregate.Render(
            renderedContent: "<html><body>Special offer for you!</body></html>",
            subject: "Limited Time Offer");

        Assert.True(renderResult.IsSuccess, $"Render failed: {renderResult.ErrorMessage}");
        Assert.Equal("Rendered", renderResult.Model!.Root.Status);

        // Step 3: Simulate failure — send without consent for marketing
        var renderedAggregate = Aggregate.CreateInstance(renderResult.Model);
        var failedSendResult = renderedAggregate.Send(hasConsent: false);

        Assert.False(failedSendResult.IsSuccess, "Marketing send without consent should fail");

        // Step 4: To simulate retry, we construct a Failed notification model
        // (In production, the status would be set to Failed by the infrastructure)
        var failedRoot = NotificationRoot.CreateInstance(
            customerId,
            campaignEventId,
            "Email",
            "tmpl-campaign-promo",
            "en-US",
            "Marketing",
            "Failed",
            retryCount: 0);

        var failedModel = new AnemicModel
        {
            Root = failedRoot,
            RenderedContent = "<html><body>Special offer for you!</body></html>",
            Subject = "Limited Time Offer"
        };

        // Step 5: Retry the failed notification
        var failedAggregate = Aggregate.CreateInstance(failedModel);
        var retryResult = failedAggregate.Retry();

        Assert.True(retryResult.IsSuccess, $"Retry failed: {retryResult.ErrorMessage}");
        Assert.Equal("Created", retryResult.Model!.Root.Status);
        Assert.Equal(1, retryResult.Model.Root.RetryCount);

        // Step 6: Re-render after retry
        var retriedAggregate = Aggregate.CreateInstance(retryResult.Model);
        var reRenderResult = retriedAggregate.Render(
            renderedContent: "<html><body>Special offer for you!</body></html>",
            subject: "Limited Time Offer");

        Assert.True(reRenderResult.IsSuccess, $"Re-render failed: {reRenderResult.ErrorMessage}");
        Assert.Equal("Rendered", reRenderResult.Model!.Root.Status);

        // Step 7: Send with consent this time
        var reRenderedAggregate = Aggregate.CreateInstance(reRenderResult.Model);
        var finalSendResult = reRenderedAggregate.Send(hasConsent: true);

        Assert.True(finalSendResult.IsSuccess, $"Final send failed: {finalSendResult.ErrorMessage}");
        Assert.Equal("Sent", finalSendResult.Model!.Root.Status);
        Assert.Equal(1, finalSendResult.Model.Root.RetryCount);
    }
}
