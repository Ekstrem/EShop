using MediatR;
using Session.InternalContracts;

namespace Session.Application.Queries;

/// <summary>
/// Query to retrieve all active sessions for a customer.
/// </summary>
public sealed class GetActiveSessionsQuery : IRequest<IReadOnlyList<SessionReadModel>>
{
    public Guid CustomerId { get; init; }
}
