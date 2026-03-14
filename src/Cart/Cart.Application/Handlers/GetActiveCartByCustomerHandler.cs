namespace Cart.Application.Handlers;

using Cart.Domain.Abstraction;
using Cart.InternalContracts;
using Cart.Application.Queries;
using MediatR;

public class GetActiveCartByCustomerHandler : IRequestHandler<GetActiveCartByCustomerQuery, ICartAnemicModel?>
{
    private readonly ICartReadRepository _readRepository;

    public GetActiveCartByCustomerHandler(ICartReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<ICartAnemicModel?> Handle(GetActiveCartByCustomerQuery request, CancellationToken ct)
        => await _readRepository.GetActiveCartByCustomerIdAsync(request.CustomerId, ct);
}
