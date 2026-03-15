using Customer.Domain;
using Customer.Domain.Abstraction;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using MediatR;

namespace Customer.Application.Commands;

/// <summary>
/// Command to change a customer's password.
/// </summary>
public sealed class ChangePasswordCommand : IRequest<AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    public Guid CustomerId { get; init; }
    public string OldPasswordHash { get; init; } = string.Empty;
    public string NewPasswordHash { get; init; } = string.Empty;
}
