using Xunit;
using Promotion.Domain.Abstraction;
using Promotion.Domain.Implementation;
using DiscountCode.Domain.Abstraction;
using DiscountCode.Domain.Implementation;
using Campaign.Domain.Abstraction;
using Campaign.Domain.Implementation;
using Notification.Domain.Abstraction;
using Notification.Domain.Implementation;

namespace EShop.IntegrationTests;

/// <summary>
/// Journey 9: Promotion → DiscountCode → Campaign → Notification
/// Verifies the marketing campaign lifecycle: create and activate a promotion,
/// generate a discount code, create and run a campaign, then send a notification.
/// </summary>
public sealed class MarketingCampaignJourneyTests
{
    [Fact]
    public void PromotionToCampaignToNotification_FullMarketingJourney_Succeeds()
    {
        // Arrange — shared identifiers
        var customerId = Guid.NewGuid();

        // ── Step 1: Create a promotion (Draft) ──────────────────────────
        var emptyPromoModel = new Promotion.Domain.Implementation.AnemicModel();
        var promoAggregate = Promotion.Domain.Implementation.Aggregate.CreateInstance(emptyPromoModel);

        var createPromoResult = promoAggregate.CreatePromotion(
            name: "Summer Sale 2026",
            description: "20% off all electronics",
            discountType: "Percentage",
            discountValue: 20m,
            startDate: DateTime.UtcNow.AddDays(-1),
            endDate: DateTime.UtcNow.AddDays(30),
            conditions: "Electronics category only",
            allowStacking: false);

        Assert.True(createPromoResult.IsSuccess, $"CreatePromotion failed: {createPromoResult.ErrorMessage}");
        Assert.Equal("Draft", createPromoResult.Model!.Root.Status);

        // ── Step 2: Activate the promotion ──────────────────────────────
        var draftPromoAggregate = Promotion.Domain.Implementation.Aggregate.CreateInstance(createPromoResult.Model);
        var activateResult = draftPromoAggregate.ActivatePromotion();

        Assert.True(activateResult.IsSuccess, $"ActivatePromotion failed: {activateResult.ErrorMessage}");
        Assert.Equal("Active", activateResult.Model!.Root.Status);

        // ── Step 3: Generate a discount code linked to the promotion ────
        var emptyCodeModel = new DiscountCode.Domain.Implementation.AnemicModel();
        var codeAggregate = DiscountCode.Domain.Implementation.Aggregate.CreateInstance(emptyCodeModel);

        var generateCodeResult = codeAggregate.GenerateDiscountCode(
            code: "SUMMER2026",
            promotionId: Guid.NewGuid(), // linked to the promotion conceptually
            maxUsage: 100,
            expiresAt: DateTime.UtcNow.AddDays(30));

        Assert.True(generateCodeResult.IsSuccess, $"GenerateDiscountCode failed: {generateCodeResult.ErrorMessage}");
        Assert.Equal("Active", generateCodeResult.Model!.Root.Status);
        Assert.Equal("SUMMER2026", generateCodeResult.Model.Root.Code);

        // ── Step 4: Create a marketing campaign (Draft) ─────────────────
        var emptyCampaignModel = new Campaign.Domain.Implementation.AnemicModel();
        var campaignAggregate = Campaign.Domain.Implementation.Aggregate.CreateInstance(emptyCampaignModel);

        var createCampaignResult = campaignAggregate.CreateCampaign(
            name: "Summer Electronics Blast",
            subject: "Save 20% on Electronics!",
            templateId: "tmpl-summer-sale",
            segmentId: "seg-electronics-buyers");

        Assert.True(createCampaignResult.IsSuccess, $"CreateCampaign failed: {createCampaignResult.ErrorMessage}");
        Assert.Equal("Draft", createCampaignResult.Model!.Root.Status);

        // ── Step 5: Schedule the campaign ────────────────────────────────
        var draftCampaignAggregate = Campaign.Domain.Implementation.Aggregate.CreateInstance(createCampaignResult.Model);
        var scheduleResult = draftCampaignAggregate.ScheduleCampaign(DateTime.UtcNow.AddDays(1));

        Assert.True(scheduleResult.IsSuccess, $"ScheduleCampaign failed: {scheduleResult.ErrorMessage}");
        Assert.Equal("Scheduled", scheduleResult.Model!.Root.Status);

        // ── Step 6: Start sending the campaign ──────────────────────────
        var scheduledCampaignAggregate = Campaign.Domain.Implementation.Aggregate.CreateInstance(scheduleResult.Model);
        var startSendResult = scheduledCampaignAggregate.StartSending(totalRecipients: 500);

        Assert.True(startSendResult.IsSuccess, $"StartSending failed: {startSendResult.ErrorMessage}");
        Assert.Equal("Sending", startSendResult.Model!.Root.Status);
        Assert.Equal(500, startSendResult.Model.TotalRecipients);

        // Complete sending
        var sendingAggregate = Campaign.Domain.Implementation.Aggregate.CreateInstance(startSendResult.Model);
        var completeSendResult = sendingAggregate.CompleteSending(sentCount: 495, failedCount: 5);

        Assert.True(completeSendResult.IsSuccess, $"CompleteSending failed: {completeSendResult.ErrorMessage}");
        Assert.Equal("Completed", completeSendResult.Model!.Root.Status);

        // ── Step 7: Create a notification for the campaign ──────────────
        var emptyNotifModel = new Notification.Domain.Implementation.AnemicModel();
        var notifAggregate = Notification.Domain.Implementation.Aggregate.CreateInstance(emptyNotifModel);

        var createNotifResult = notifAggregate.CreateNotification(
            customerId: customerId,
            eventId: Guid.NewGuid(),
            channel: "Email",
            templateId: "tmpl-summer-sale",
            locale: "en-US",
            type: "Marketing");

        Assert.True(createNotifResult.IsSuccess, $"CreateNotification failed: {createNotifResult.ErrorMessage}");
        Assert.Equal("Created", createNotifResult.Model!.Root.Status);

        // ── Step 8: Render the notification ─────────────────────────────
        var createdNotifAggregate = Notification.Domain.Implementation.Aggregate.CreateInstance(createNotifResult.Model);
        var renderResult = createdNotifAggregate.Render(
            renderedContent: "<html><body>Save 20% on Electronics with code SUMMER2026!</body></html>",
            subject: "Save 20% on Electronics!");

        Assert.True(renderResult.IsSuccess, $"Render failed: {renderResult.ErrorMessage}");
        Assert.Equal("Rendered", renderResult.Model!.Root.Status);

        // ── Step 9: Send the notification (with consent=true) ───────────
        var renderedNotifAggregate = Notification.Domain.Implementation.Aggregate.CreateInstance(renderResult.Model);
        var sendResult = renderedNotifAggregate.Send(hasConsent: true);

        Assert.True(sendResult.IsSuccess, $"Send failed: {sendResult.ErrorMessage}");
        Assert.Equal("Sent", sendResult.Model!.Root.Status);

        // ── Final assertions ────────────────────────────────────────────
        Assert.Equal("Active", activateResult.Model.Root.Status);
        Assert.Equal("Active", generateCodeResult.Model.Root.Status);
        Assert.Equal("Completed", completeSendResult.Model.Root.Status);
        Assert.Equal("Sent", sendResult.Model.Root.Status);
    }
}
