using Customer.InternalContracts;
using MediatR;

namespace Customer.Application.Queries;

public sealed class GetCustomerProfileQueryHandler
    : IRequestHandler<GetCustomerProfileQuery, CustomerReadModel?>
{
    private readonly ICustomerQueryRepository _queryRepository;

    public GetCustomerProfileQueryHandler(ICustomerQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<CustomerReadModel?> Handle(
        GetCustomerProfileQuery request,
        CancellationToken ct)
    {
        return await _queryRepository.GetByIdAsync(request.CustomerId, ct);
    }
}
