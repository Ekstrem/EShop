namespace Category.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Category.Domain;
using Category.Domain.Abstraction;

public sealed record CreateCategoryCommand(
    string Name,
    Guid? ParentId,
    int SortOrder)
    : IRequest<AggregateResult<ICategory, ICategoryAnemicModel>>;
