using EShop.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Session.Application.Commands;
using Session.Application.Queries;

namespace Session.Api;

[ApiController]
[Route("api/[controller]")]
public sealed class SessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SessionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new session (login).
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateSessionCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(
            nameof(GetActiveSessions),
            new { customerId = result.Model().Root.CustomerId },
            new
            {
                SessionId = result.Model().Id,
                Token = result.Model().Root.Token,
                ExpiresAt = result.Model().Root.ExpiresAt
            });
    }

    /// <summary>
    /// Refresh an existing session.
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshSessionCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(new
        {
            SessionId = result.Model().Id,
            Token = result.Model().Root.Token,
            ExpiresAt = result.Model().Root.ExpiresAt
        });
    }

    /// <summary>
    /// Revoke a single session (logout).
    /// </summary>
    [HttpPost("revoke/{id:guid}")]
    public async Task<IActionResult> Revoke(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new RevokeSessionCommand { SessionId = id }, ct);
        return NoContent();
    }

    /// <summary>
    /// Revoke all sessions for a customer.
    /// </summary>
    [HttpPost("revoke-all/{customerId:guid}")]
    public async Task<IActionResult> RevokeAll(Guid customerId, CancellationToken ct)
    {
        var count = await _mediator.Send(
            new RevokeAllSessionsCommand { CustomerId = customerId }, ct);
        return Ok(new { RevokedCount = count });
    }

    /// <summary>
    /// Get all active sessions for a customer.
    /// </summary>
    [HttpGet("active/{customerId:guid}")]
    public async Task<IActionResult> GetActiveSessions(Guid customerId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetActiveSessionsQuery { CustomerId = customerId }, ct);
        return Ok(result);
    }
}
