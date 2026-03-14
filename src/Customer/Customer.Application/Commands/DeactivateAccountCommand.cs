using Customer.Domain;
using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;

namespace Customer.Application.Commands;

/// <summary>
/// Command to deactivate a customer account.
/// </summary>
public sealed class DeactivateAccountCommand : IRequest<AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    public Guid CustomerId { get; init; }
}
