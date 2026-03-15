using Customer.Domain;
using Customer.Domain.Abstraction;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
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
