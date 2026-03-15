namespace Product.Application.Commands;

using MediatR;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using EShop.Contracts;
using Product.Domain;
using Product.Domain.Abstraction;
using Product.Domain.Implementation;
using Product.DomainServices;

public sealed class CreateProductHandler
    : IRequestHandler<CreateProductCommand, AggregateResult<IProduct, IProductAnemicModel>>
{
    private readonly AggregateProvider _provider;

    public CreateProductHandler(AggregateProvider provider)
    {
        _provider = provider;
    }

    public Task<AggregateResult<IProduct, IProductAnemicModel>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var variants = request.Variants
            .Select(v => ProductVariant.CreateInstance(v.Sku, v.Size, v.Color, v.Price))
            .ToList();

        var media = request.Media
            .Select(m => ProductMedia.CreateInstance(m.Url, m.Alt, m.SortOrder))
            .ToList();

        var emptyModel = new AnemicModel();
        var aggregate = Aggregate.CreateInstance(emptyModel);
        var result = aggregate.CreateProduct(
            request.Name,
            request.Description,
            request.CategoryId,
            variants,
            media);

        return Task.FromResult(result);
    }
}
