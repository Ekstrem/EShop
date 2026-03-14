namespace Order.Application.Handlers;

using Order.Domain.Abstraction;
using Order.InternalContracts;
using Order.Application.Queries;
using MediatR;

public class ListOrdersHandler : IRequestHandler<ListOrdersQuery, IReadOnlyList<IOrderAnemicModel>>
{
    private readonly IOrderReadRepository _readRepository;

    public ListOrdersHandler(IOrderReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IReadOnlyList<IOrderAnemicModel>> Handle(ListOrdersQuery request, CancellationToken ct)
        => await _readRepository.GetByCustomerIdAsync(request.CustomerId, ct);
}
