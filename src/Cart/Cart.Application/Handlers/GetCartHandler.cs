namespace Cart.Application.Handlers;

using Cart.Domain.Abstraction;
using Cart.InternalContracts;
using Cart.Application.Queries;
using MediatR;

public class GetCartHandler : IRequestHandler<GetCartQuery, ICartAnemicModel?>
{
    private readonly ICartReadRepository _readRepository;

    public GetCartHandler(ICartReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<ICartAnemicModel?> Handle(GetCartQuery request, CancellationToken ct)
        => await _readRepository.GetByIdAsync(request.CartId, ct);
}
