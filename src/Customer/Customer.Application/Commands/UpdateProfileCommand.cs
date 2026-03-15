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
/// Command to update a customer's profile.
/// </summary>
public sealed class UpdateProfileCommand : IRequest<AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    public Guid CustomerId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public List<AddressDto> Addresses { get; init; } = new();
}

public sealed class AddressDto
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public bool IsDefault { get; init; }
}
