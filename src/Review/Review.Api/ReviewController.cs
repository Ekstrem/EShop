namespace Review.Api;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Review.Application.Commands;
using Review.Application.Queries;
using Review.InternalContracts;

[ApiController]
[Route("api/[controller]")]
public sealed class ReviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Submit(
        [FromBody] SubmitReviewCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{reviewId:guid}/edit")]
    public async Task<IActionResult> Edit(
        Guid reviewId,
        [FromBody] EditReviewCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { ReviewId = reviewId }, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> Delete(
        Guid reviewId,
        [FromQuery] Guid requesterId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new DeleteReviewCommand(reviewId, requesterId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{reviewId:guid}/approve")]
    public async Task<IActionResult> Approve(
        Guid reviewId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ApproveReviewCommand(reviewId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{reviewId:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid reviewId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RejectReviewCommand(reviewId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{reviewId:guid}/flag")]
    public async Task<IActionResult> Flag(
        Guid reviewId,
        [FromBody] FlagReviewCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with { ReviewId = reviewId }, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{reviewId:guid}/vote-helpful")]
    public async Task<IActionResult> VoteHelpful(
        Guid reviewId,
        [FromBody] VoteHelpfulCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with { ReviewId = reviewId }, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{reviewId:guid}/respond")]
    public async Task<IActionResult> Respond(
        Guid reviewId,
        [FromBody] RespondToReviewCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command with { ReviewId = reviewId }, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{reviewId:guid}")]
    public async Task<IActionResult> GetById(
        Guid reviewId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetReviewQuery(reviewId), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(
        Guid productId,
        [FromQuery] string? status,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetReviewsByProductQuery(productId, status, skip, take), cancellationToken);
        return Ok(result);
    }
}
