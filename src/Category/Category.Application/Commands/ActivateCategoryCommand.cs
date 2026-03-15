namespace Category.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using Category.Domain;
using Category.Domain.Abstraction;

public sealed record ActivateCategoryCommand(Guid CategoryId)
    : IRequest<AggregateResult<ICategory, ICategoryAnemicModel>>;
