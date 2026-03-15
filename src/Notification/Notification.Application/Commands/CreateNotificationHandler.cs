namespace Notification.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Notification.Domain;
using Notification.Domain.Abstraction;
using Notification.Domain.Implementation;
using INotification = Notification.Domain.INotification;

public sealed class CreateNotificationHandler
    : IRequestHandler<CreateNotificationCommand, AggregateResult<INotification, INotificationAnemicModel>>
{
    public Task<AggregateResult<INotification, INotificationAnemicModel>> Handle(
        CreateNotificationCommand request,
        CancellationToken cancellationToken)
    {
        var emptyModel = new AnemicModel();
        var aggregate = Aggregate.CreateInstance(emptyModel);
        var result = aggregate.CreateNotification(
            request.CustomerId,
            request.EventId,
            request.Channel,
            request.TemplateId,
            request.Locale,
            request.Type);

        return Task.FromResult(result);
    }
}
