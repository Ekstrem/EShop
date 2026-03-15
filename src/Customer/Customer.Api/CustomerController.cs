using Customer.Application.Commands;
using Customer.Application.Queries;
using Customer.InternalContracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Customer.Api;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Register a new customer.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCustomerCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(
            nameof(GetProfile),
            new { id = result.Model.Id },
            new { Id = result.Model.Id });
    }

    /// <summary>
    /// Verify a customer's email.
    /// </summary>
    [HttpPost("verify")]
    public async Task<IActionResult> Verify(
        [FromBody] VerifyEmailCommand command,
        CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return Ok();
    }

    /// <summary>
    /// Update a customer's profile.
    /// </summary>
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileCommand command,
        CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Change a customer's password.
    /// </summary>
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordCommand command,
        CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Deactivate a customer account.
    /// </summary>
    [HttpDelete("deactivate/{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeactivateAccountCommand { CustomerId = id }, ct);
        return NoContent();
    }

    /// <summary>
    /// Get a customer profile by ID.
    /// </summary>
    [HttpGet("profile/{id:guid}")]
    public async Task<IActionResult> GetProfile(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetCustomerProfileQuery { CustomerId = id }, ct);

        if (result is null) return NotFound();
        return Ok(result);
    }
}
