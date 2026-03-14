using Customer.InternalContracts;
using MediatR;

namespace Customer.Application.Queries;

/// <summary>
/// Query to retrieve a customer profile by ID.
/// </summary>
public sealed class GetCustomerProfileQuery : IRequest<CustomerReadModel?>
{
    public Guid CustomerId { get; init; }
}
