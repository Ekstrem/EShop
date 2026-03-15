using Customer.Domain;
using Customer.Domain.Abstraction;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;

namespace Customer.Application.Commands;

/// <summary>
/// Command to register a new customer.
/// </summary>
public sealed class RegisterCustomerCommand : IRequest<AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
}
