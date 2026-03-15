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
/// Command to deactivate a customer account.
/// </summary>
public sealed class DeactivateAccountCommand : IRequest<AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    public Guid CustomerId { get; init; }
}
