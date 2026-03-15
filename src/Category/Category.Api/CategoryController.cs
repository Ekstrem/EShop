using EShop.Contracts;
namespace Category.Api;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Category.Application.Commands;
using Category.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var cmd = command with { CategoryId = id };
        var result = await _mediator.Send(cmd, cancellationToken);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPost("{id:guid}/move")]
    public async Task<IActionResult> Move(
        Guid id,
        [FromBody] MoveCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var cmd = command with { CategoryId = id };
        var result = await _mediator.Send(cmd, cancellationToken);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeactivateCategoryCommand(id), cancellationToken);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ActivateCategoryCommand(id), cancellationToken);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoryQuery(id), cancellationToken);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("tree")]
    public async Task<IActionResult> GetTree(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoryTreeQuery(), cancellationToken);
        return Ok(result);
    }
}
