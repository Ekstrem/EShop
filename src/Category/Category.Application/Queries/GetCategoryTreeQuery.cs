namespace Category.Application.Queries;

using MediatR;
using Category.InternalContracts;

public sealed record GetCategoryTreeQuery()
    : IRequest<IReadOnlyList<CategoryReadModel>>;

public sealed class GetCategoryTreeHandler
    : IRequestHandler<GetCategoryTreeQuery, IReadOnlyList<CategoryReadModel>>
{
    private readonly ICategoryQueryRepository _repository;

    public GetCategoryTreeHandler(ICategoryQueryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CategoryReadModel>> Handle(
        GetCategoryTreeQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetTreeAsync(cancellationToken);
    }
}
