namespace Notification.Application.Queries;

using MediatR;
using Notification.InternalContracts;

public sealed record SearchNotificationsQuery(
    Guid? CustomerId,
    string? Channel,
    string? Status,
    int Skip,
    int Take)
    : IRequest<IReadOnlyList<NotificationReadModel>>;
