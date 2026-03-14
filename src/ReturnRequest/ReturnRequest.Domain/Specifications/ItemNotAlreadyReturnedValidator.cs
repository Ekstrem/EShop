using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that items in the return request have not already been returned
/// in a previous return request.
/// </summary>
public sealed class ItemNotAlreadyReturnedValidator : IBusinessOperationValidator<IReturnRequest, IReturnRequestAnemicModel>
{
    private readonly IReadOnlyList<Guid> _alreadyReturnedVariantIds;

    public ItemNotAlreadyReturnedValidator(IReadOnlyList<Guid> alreadyReturnedVariantIds)
    {
        _alreadyReturnedVariantIds = alreadyReturnedVariantIds;
    }

    public bool IsValid(IReturnRequestAnemicModel model)
    {
        return !model.Items.Any(item =>
            _alreadyReturnedVariantIds.Contains(item.VariantId));
    }

    public string ErrorMessage =>
        "One or more items have already been returned in a previous return request.";
}
