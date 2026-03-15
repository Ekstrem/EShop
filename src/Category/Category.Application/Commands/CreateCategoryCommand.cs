namespace Category.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Category.Domain;
using Category.Domain.Abstraction;

public sealed record CreateCategoryCommand(
    string Name,
    Guid? ParentId,
    int SortOrder)
    : IRequest<AggregateResult<ICategory, ICategoryAnemicModel>>;
