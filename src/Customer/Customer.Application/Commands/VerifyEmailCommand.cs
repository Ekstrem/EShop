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
/// Command to verify a customer's email address.
/// </summary>
public sealed class VerifyEmailCommand : IRequest<AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    public Guid CustomerId { get; init; }
}
