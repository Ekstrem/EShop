namespace AggregateRating.Api;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using AggregateRating.Application.Commands;
using AggregateRating.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public sealed class AggregateRatingController : ControllerBase
{
    private readonly IMediator _mediator;

    public AggregateRatingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize(
        [FromBody] InitializeRatingCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("recalculate")]
    public async Task<IActionResult> Recalculate(
        [FromBody] RecalculateRatingCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAggregateRatingQuery(productId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
