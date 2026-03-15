using EShop.Contracts;
namespace StockItem.Api;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockItem.Application.Commands;
using StockItem.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public sealed class StockItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{id:guid}/reserve")]
    public async Task<IActionResult> Reserve(
        Guid id,
        [FromBody] ReserveStockRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveStockCommand(id, request.OrderId, request.Quantity);
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPost("{id:guid}/release")]
    public async Task<IActionResult> Release(
        Guid id,
        [FromBody] ReleaseStockRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReleaseStockCommand(id, request.OrderId);
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPost("{id:guid}/replenish")]
    public async Task<IActionResult> Replenish(
        Guid id,
        [FromBody] ReplenishStockRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReplenishStockCommand(id, request.Quantity);
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPost("{id:guid}/adjust")]
    public async Task<IActionResult> Adjust(
        Guid id,
        [FromBody] AdjustStockRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AdjustStockCommand(id, request.NewTotal);
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetStockItemQuery(id), cancellationToken);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLowStockItemsQuery(), cancellationToken);
        return Ok(result);
    }
}

public sealed record ReserveStockRequest(Guid OrderId, int Quantity);
public sealed record ReleaseStockRequest(Guid OrderId);
public sealed record ReplenishStockRequest(int Quantity);
public sealed record AdjustStockRequest(int NewTotal);
