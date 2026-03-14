namespace Notification.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Commands;
using Notification.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public sealed class NotificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateNotificationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/render")]
    public async Task<IActionResult> Render(
        Guid id,
        [FromBody] RenderNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RenderNotificationCommand(id, request.RenderedContent, request.Subject);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/send")]
    public async Task<IActionResult> Send(
        Guid id,
        [FromBody] SendNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SendNotificationCommand(id, request.HasConsent);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/retry")]
    public async Task<IActionResult> Retry(Guid id, CancellationToken cancellationToken)
    {
        var command = new RetryNotificationCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetNotificationQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] Guid? customerId,
        [FromQuery] string? channel,
        [FromQuery] string? status,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchNotificationsQuery(customerId, channel, status, skip, take);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

public sealed record RenderNotificationRequest(string RenderedContent, string Subject);
public sealed record SendNotificationRequest(bool HasConsent);
