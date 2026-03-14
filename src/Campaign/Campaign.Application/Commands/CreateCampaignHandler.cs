namespace Campaign.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Campaign.Domain;
using Campaign.Domain.Abstraction;
using Campaign.Domain.Implementation;

public sealed class CreateCampaignHandler
    : IRequestHandler<CreateCampaignCommand, AggregateResult<ICampaign, ICampaignAnemicModel>>
{
    public Task<AggregateResult<ICampaign, ICampaignAnemicModel>> Handle(
        CreateCampaignCommand request,
        CancellationToken cancellationToken)
    {
        var emptyModel = new AnemicModel();
        var aggregate = Aggregate.CreateInstance(emptyModel);
        var result = aggregate.CreateCampaign(
            request.Name,
            request.Subject,
            request.TemplateId,
            request.SegmentId);

        return Task.FromResult(result);
    }
}
