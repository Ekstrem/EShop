namespace DiscountCode.Application.Queries;

using MediatR;
using DiscountCode.InternalContracts;

public sealed class GetDiscountCodeHandler : IRequestHandler<GetDiscountCodeQuery, DiscountCodeReadModel?>
{
    private readonly IDiscountCodeQueryRepository _repository;

    public GetDiscountCodeHandler(IDiscountCodeQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<DiscountCodeReadModel?> Handle(
        GetDiscountCodeQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.DiscountCodeId, cancellationToken);
    }
}
