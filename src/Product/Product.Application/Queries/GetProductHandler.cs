namespace Product.Application.Queries;

using MediatR;
using Product.InternalContracts;

public sealed class GetProductHandler : IRequestHandler<GetProductQuery, ProductReadModel?>
{
    private readonly IProductQueryRepository _repository;

    public GetProductHandler(IProductQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductReadModel?> Handle(
        GetProductQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.ProductId, cancellationToken);
    }
}
