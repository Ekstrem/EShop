namespace Notification.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Notification.Domain;
using Notification.Domain.Abstraction;
using INotification = Notification.Domain.INotification;

public sealed record RetryNotificationCommand(Guid NotificationId)
    : IRequest<AggregateResult<INotification, INotificationAnemicModel>>;
