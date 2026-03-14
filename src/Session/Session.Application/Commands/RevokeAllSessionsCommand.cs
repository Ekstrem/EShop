using MediatR;

namespace Session.Application.Commands;

/// <summary>
/// Command to revoke all active sessions for a customer.
/// </summary>
public sealed class RevokeAllSessionsCommand : IRequest<int>
{
    public Guid CustomerId { get; init; }
}
