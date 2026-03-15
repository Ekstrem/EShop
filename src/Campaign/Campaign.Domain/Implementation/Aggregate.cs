namespace Campaign.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Campaign.Domain.Abstraction;
using Campaign.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public ICampaignAnemicModel Model { get; }

    private Aggregate(ICampaignAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(ICampaignAnemicModel model) => new(model);

    private AggregateResult<ICampaign, ICampaignAnemicModel> Success(ICampaignAnemicModel newModel)
    {
        var data = BusinessOperationData<ICampaign, ICampaignAnemicModel>
            .Commit<ICampaign, ICampaignAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<ICampaign, ICampaignAnemicModel>(data);
    }

    private AggregateResult<ICampaign, ICampaignAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<ICampaign, ICampaignAnemicModel>
            .Commit<ICampaign, ICampaignAnemicModel>(Model, Model);
        return new AggregateResultException<ICampaign, ICampaignAnemicModel>(
            data, new FailedSpecification<ICampaign, ICampaignAnemicModel>(error));
    }

    public AggregateResult<ICampaign, ICampaignAnemicModel> CreateCampaign(
        string name,
        string subject,
        string templateId,
        string segmentId)
    {
        var root = CampaignRoot.CreateInstance(name, subject, templateId, segmentId);
        var anemic = new AnemicModel
        {
            Root = root,
            TotalRecipients = 0,
            SentCount = 0,
            FailedCount = 0
        };

        return Success(anemic);
    }

    public AggregateResult<ICampaign, ICampaignAnemicModel> UpdateCampaign(
        string name,
        string subject,
        string templateId,
        string segmentId)
    {
        var validator = new IsDraftValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Only draft campaigns can be updated.");

        var root = CampaignRoot.CreateInstance(
            name, subject, templateId, segmentId,
            Model.Root.ScheduledAt, "Draft", Model.Root.CreatedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            TotalRecipients = Model.TotalRecipients,
            SentCount = Model.SentCount,
            FailedCount = Model.FailedCount
        };

        return Success(anemic);
    }

    public AggregateResult<ICampaign, ICampaignAnemicModel> ScheduleCampaign(DateTime scheduledAt)
    {
        var draftValidator = new IsDraftValidator();
        if (!draftValidator.IsSatisfiedBy(Model))
            return Fail("Only draft campaigns can be scheduled.");

        var requiredFields = new HasRequiredFieldsValidator();
        if (!requiredFields.IsSatisfiedBy(Model))
            return Fail("Campaign must have templateId, subject, and segmentId.");

        var scheduleValidator = new ScheduleInFutureValidator(scheduledAt);
        if (!scheduleValidator.IsSatisfiedBy(Model))
            return Fail("Scheduled date must be in the future.");

        var root = CampaignRoot.CreateInstance(
            Model.Root.Name, Model.Root.Subject, Model.Root.TemplateId,
            Model.Root.SegmentId, scheduledAt, "Scheduled", Model.Root.CreatedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            TotalRecipients = Model.TotalRecipients,
            SentCount = Model.SentCount,
            FailedCount = Model.FailedCount
        };

        return Success(anemic);
    }

    public AggregateResult<ICampaign, ICampaignAnemicModel> CancelCampaign()
    {
        var validator = new CanCancelValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Only Draft or Scheduled campaigns can be cancelled.");

        var root = CampaignRoot.CreateInstance(
            Model.Root.Name, Model.Root.Subject, Model.Root.TemplateId,
            Model.Root.SegmentId, Model.Root.ScheduledAt, "Cancelled", Model.Root.CreatedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            TotalRecipients = Model.TotalRecipients,
            SentCount = Model.SentCount,
            FailedCount = Model.FailedCount
        };

        return Success(anemic);
    }

    public AggregateResult<ICampaign, ICampaignAnemicModel> StartSending(int totalRecipients)
    {
        var validator = new IsScheduledValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Only scheduled campaigns can start sending.");

        var root = CampaignRoot.CreateInstance(
            Model.Root.Name, Model.Root.Subject, Model.Root.TemplateId,
            Model.Root.SegmentId, Model.Root.ScheduledAt, "Sending", Model.Root.CreatedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            TotalRecipients = totalRecipients,
            SentCount = 0,
            FailedCount = 0
        };

        return Success(anemic);
    }

    public AggregateResult<ICampaign, ICampaignAnemicModel> CompleteSending(int sentCount, int failedCount)
    {
        if (Model.Root.Status != "Sending")
            return Fail("Only campaigns in Sending status can be completed.");

        var root = CampaignRoot.CreateInstance(
            Model.Root.Name, Model.Root.Subject, Model.Root.TemplateId,
            Model.Root.SegmentId, Model.Root.ScheduledAt, "Completed", Model.Root.CreatedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            TotalRecipients = Model.TotalRecipients,
            SentCount = sentCount,
            FailedCount = failedCount
        };

        return Success(anemic);
    }
}
