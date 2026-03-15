namespace Notification.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Notification.Domain;
using Notification.Domain.Abstraction;
using INotification = Notification.Domain.INotification;

public sealed record CreateNotificationCommand(
    Guid CustomerId,
    Guid EventId,
    string Channel,
    string TemplateId,
    string Locale,
    string Type)
    : IRequest<AggregateResult<INotification, INotificationAnemicModel>>;
