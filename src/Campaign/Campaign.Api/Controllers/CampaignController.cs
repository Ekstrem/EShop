namespace Campaign.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Campaign.Application.Commands;
using Campaign.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public sealed class CampaignController : ControllerBase
{
    private readonly IMediator _mediator;

    public CampaignController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCampaignCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCampaignRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCampaignCommand(id, request.Name, request.Subject, request.TemplateId, request.SegmentId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/schedule")]
    public async Task<IActionResult> Schedule(
        Guid id,
        [FromBody] ScheduleCampaignRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ScheduleCampaignCommand(id, request.ScheduledAt);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var command = new CancelCampaignCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/start-sending")]
    public async Task<IActionResult> StartSending(
        Guid id,
        [FromBody] StartSendingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new StartSendingCommand(id, request.TotalRecipients);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/complete-sending")]
    public async Task<IActionResult> CompleteSending(
        Guid id,
        [FromBody] CompleteSendingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CompleteSendingCommand(id, request.SentCount, request.FailedCount);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCampaignQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] string? status,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchCampaignsQuery(name, status, skip, take);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

public sealed record UpdateCampaignRequest(string Name, string Subject, string TemplateId, string SegmentId);
public sealed record ScheduleCampaignRequest(DateTime ScheduledAt);
public sealed record StartSendingRequest(int TotalRecipients);
public sealed record CompleteSendingRequest(int SentCount, int FailedCount);
