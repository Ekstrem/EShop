namespace Order.Application.Handlers;

using Order.Domain.Abstraction;
using Order.InternalContracts;
using Order.Application.Queries;
using MediatR;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, IOrderAnemicModel?>
{
    private readonly IOrderReadRepository _readRepository;

    public GetOrderHandler(IOrderReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IOrderAnemicModel?> Handle(GetOrderQuery request, CancellationToken ct)
        => await _readRepository.GetByIdAsync(request.OrderId, ct);
}
