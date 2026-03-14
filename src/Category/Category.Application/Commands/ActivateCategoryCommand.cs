namespace Category.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Category.Domain;
using Category.Domain.Abstraction;

public sealed record ActivateCategoryCommand(Guid CategoryId)
    : IRequest<AggregateResult<ICategory, ICategoryAnemicModel>>;
