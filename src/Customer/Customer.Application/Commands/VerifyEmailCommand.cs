using Customer.Domain;
using Customer.Domain.Abstraction;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;

namespace Customer.Application.Commands;

/// <summary>
/// Command to verify a customer's email address.
/// </summary>
public sealed class VerifyEmailCommand : IRequest<AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    public Guid CustomerId { get; init; }
}
