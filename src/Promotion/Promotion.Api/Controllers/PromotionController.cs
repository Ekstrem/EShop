using EShop.Contracts;
namespace Promotion.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promotion.Application.Commands;
using Promotion.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public sealed class PromotionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PromotionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePromotionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePromotionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePromotionCommand(
            id, request.Name, request.Description, request.DiscountType,
            request.DiscountValue, request.StartDate, request.EndDate,
            request.Conditions, request.AllowStacking);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        var command = new ActivatePromotionCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivatePromotionCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPromotionQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] string? discountType,
        [FromQuery] string? status,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchPromotionsQuery(name, discountType, status, skip, take);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

public sealed record UpdatePromotionRequest(
    string Name,
    string Description,
    string DiscountType,
    decimal DiscountValue,
    DateTime StartDate,
    DateTime EndDate,
    string Conditions,
    bool AllowStacking);
