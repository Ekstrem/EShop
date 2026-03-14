namespace Category.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Category.Domain;
using Category.Domain.Abstraction;
using Category.Domain.Implementation;
using Category.InternalContracts;

public sealed class CreateCategoryHandler
    : IRequestHandler<CreateCategoryCommand, AggregateResult<ICategory, ICategoryAnemicModel>>
{
    private readonly ICategoryQueryRepository _queryRepository;

    public CreateCategoryHandler(ICategoryQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<AggregateResult<ICategory, ICategoryAnemicModel>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var siblingNames = await _queryRepository.GetSiblingNamesAsync(
            request.ParentId, null, cancellationToken);

        int depth = 0;
        if (request.ParentId.HasValue)
        {
            var parent = await _queryRepository.GetByIdAsync(request.ParentId.Value, cancellationToken);
            if (parent is null)
                return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Parent category not found.");
            depth = parent.Depth + 1;
        }

        var emptyModel = new AnemicModel();
        var aggregate = Aggregate.CreateInstance(emptyModel);

        return aggregate.CreateCategory(
            request.Name,
            request.ParentId,
            depth,
            request.SortOrder,
            siblingNames);
    }
}
