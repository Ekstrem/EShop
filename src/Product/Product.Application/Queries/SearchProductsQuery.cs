namespace Product.Application.Queries;

using MediatR;
using Product.InternalContracts;

public sealed record SearchProductsQuery(
    string? Name,
    Guid? CategoryId,
    string? Status,
    int Skip = 0,
    int Take = 20)
    : IRequest<IReadOnlyList<ProductReadModel>>;

public sealed class SearchProductsHandler
    : IRequestHandler<SearchProductsQuery, IReadOnlyList<ProductReadModel>>
{
    private readonly IProductQueryRepository _repository;

    public SearchProductsHandler(IProductQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<ProductReadModel>> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.SearchAsync(
            request.Name,
            request.CategoryId,
            request.Status,
            request.Skip,
            request.Take,
            cancellationToken);
    }
}
