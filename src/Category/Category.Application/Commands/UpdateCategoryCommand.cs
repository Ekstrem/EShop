namespace Category.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Category.Domain;
using Category.Domain.Abstraction;

public sealed record UpdateCategoryCommand(
    Guid CategoryId,
    string Name,
    int SortOrder)
    : IRequest<AggregateResult<ICategory, ICategoryAnemicModel>>;
