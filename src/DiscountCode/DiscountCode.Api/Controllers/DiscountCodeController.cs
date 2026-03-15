using EShop.Contracts;
namespace DiscountCode.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using DiscountCode.Application.Commands;
using DiscountCode.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public sealed class DiscountCodeController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountCodeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Generate(
        [FromBody] GenerateDiscountCodeCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id:guid}/validate")]
    public async Task<IActionResult> Validate(Guid id, CancellationToken cancellationToken)
    {
        var command = new ValidateDiscountCodeCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id:guid}/redeem")]
    public async Task<IActionResult> Redeem(
        Guid id,
        [FromBody] RedeemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RedeemDiscountCodeCommand(id, request.OrderId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id:guid}/compensate")]
    public async Task<IActionResult> Compensate(
        Guid id,
        [FromBody] CompensateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CompensateRedemptionCommand(id, request.OrderId);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivateCodeCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess() ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDiscountCodeQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? code,
        [FromQuery] Guid? promotionId,
        [FromQuery] string? status,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchDiscountCodesQuery(code, promotionId, status, skip, take);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

public sealed record RedeemRequest(Guid OrderId);
public sealed record CompensateRequest(Guid OrderId);
