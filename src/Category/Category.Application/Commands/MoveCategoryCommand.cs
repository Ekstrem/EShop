namespace Category.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Category.Domain;
using Category.Domain.Abstraction;

public sealed record MoveCategoryCommand(
    Guid CategoryId,
    Guid? NewParentId,
    int SortOrder)
    : IRequest<AggregateResult<ICategory, ICategoryAnemicModel>>;
