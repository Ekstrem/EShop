namespace Category.Application.Queries;

using MediatR;
using Category.InternalContracts;

public sealed record GetCategoryQuery(Guid CategoryId)
    : IRequest<CategoryReadModel?>;

public sealed class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryReadModel?>
{
    private readonly ICategoryQueryRepository _repository;

    public GetCategoryHandler(ICategoryQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryReadModel?> Handle(
        GetCategoryQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.CategoryId, cancellationToken);
    }
}
