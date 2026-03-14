namespace Product.Application.Commands;

using MediatR;
using Hive.SeedWorks.Result;
using Product.Domain;
using Product.Domain.Abstraction;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    Guid CategoryId,
    IReadOnlyList<CreateProductVariantDto> Variants,
    IReadOnlyList<CreateProductMediaDto> Media)
    : IRequest<AggregateResult<IProduct, IProductAnemicModel>>;

public sealed record CreateProductVariantDto(
    string Sku,
    string Size,
    string Color,
    decimal Price);

public sealed record CreateProductMediaDto(
    string Url,
    string Alt,
    int SortOrder);
