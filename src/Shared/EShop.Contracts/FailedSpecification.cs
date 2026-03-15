namespace EShop.Contracts;

using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

/// <summary>
/// A specification that always fails with the given reason.
/// Used to construct AggregateResultException for validation failures.
/// </summary>
public sealed class FailedSpecification<TBC, TModel> : IBusinessOperationSpecification<TBC, TModel>
    where TBC : IBoundedContext
    where TModel : IAnemicModel<TBC>
{
    public FailedSpecification(string reason) => Reason = reason;

    public bool IsSatisfiedBy(BusinessOperationData<TBC, TModel> obj) => false;

    public string Reason { get; }

    public DomainOperationResultEnum DomainResult => DomainOperationResultEnum.Exception;
}
