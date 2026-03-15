namespace Product.Api;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Commands;
using Product.Application.Queries;
using Product.InternalContracts;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Model);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var cmd = command with { ProductId = id };
        var result = await _mediator.Send(cmd, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Model);
    }

    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> Publish(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new PublishProductCommand(id), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Model);
    }

    [HttpPost("{id:guid}/archive")]
    public async Task<IActionResult> Archive(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ArchiveProductCommand(id), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Model);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductQuery(id), cancellationToken);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] Guid? categoryId,
        [FromQuery] string? status,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new SearchProductsQuery(name, categoryId, status, skip, take),
            cancellationToken);

        return Ok(result);
    }
}
