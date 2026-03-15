namespace EShop.Contracts;

using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;

/// <summary>Command-side database context for persisting aggregate state.</summary>
public interface ICommandDbContext<TBC> where TBC : IBoundedContext
{
    Task SaveAsync(IAnemicModel<TBC> model, CancellationToken cancellationToken = default);
}

/// <summary>Read model marker.</summary>
public interface IReadModel<TBC> where TBC : IBoundedContext { }

/// <summary>Read repository.</summary>
public interface IReadRepository<TBC, TModel>
    where TBC : IBoundedContext
    where TModel : IReadModel<TBC>
{
    Task<TModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken ct = default);
}

/// <summary>Repository for command-side persistence.</summary>
public interface IRepository<TBC, TModel>
    where TBC : IBoundedContext
    where TModel : IAnemicModel<TBC>
{
    Task<TModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task SaveAsync(AggregateResult<TBC, TModel> result, CancellationToken ct = default);
}

/// <summary>Bus adapter for a specific bounded context.</summary>
public interface IBusAdapter<TBC> where TBC : IBoundedContext
{
    Task Publish<TEvent>(TEvent @event, CancellationToken ct = default);
}

/// <summary>Notifier for aggregate result distribution.</summary>
public interface INotifier<TBC> where TBC : IBoundedContext
{
    void Notify<TModel>(AggregateResult<TBC, TModel> result)
        where TModel : IAnemicModel<TBC>;
}

/// <summary>
/// Extension methods providing convenience accessors on AggregateResult,
/// bridging the DigiTFactory library API to the simpler patterns used in EShop.
/// </summary>
public static class AggregateResultExtensions
{
    /// <summary>Returns true if the aggregate operation succeeded.</summary>
    public static bool IsSuccess<TBC, TModel>(this AggregateResult<TBC, TModel> result)
        where TBC : IBoundedContext
        where TModel : IAnemicModel<TBC>
        => result.Result == DomainOperationResultEnum.Success;

    /// <summary>Returns the resulting model from the aggregate operation.</summary>
    public static TModel Model<TBC, TModel>(this AggregateResult<TBC, TModel> result)
        where TBC : IBoundedContext
        where TModel : IAnemicModel<TBC>
        => result.BusinessOperationData.Model;

    /// <summary>Returns the first error message, or null if none.</summary>
    public static string? ErrorMessage<TBC, TModel>(this AggregateResult<TBC, TModel> result)
        where TBC : IBoundedContext
        where TModel : IAnemicModel<TBC>
        => result.Reason?.FirstOrDefault();

    /// <summary>Creates a successful AggregateResult with the given model and event name.</summary>
    public static AggregateResult<TBC, TModel> CreateResult<TBC, TModel>(
        TModel model, string eventName)
        where TBC : IBoundedContext
        where TModel : IAnemicModel<TBC>
    {
        var data = BusinessOperationData<TBC, TModel>
            .Commit<TBC, TModel>(model, model);
        return new AggregateResultSuccess<TBC, TModel>(data);
    }

    /// <summary>Creates a failed AggregateResult with the given error message.</summary>
    public static AggregateResult<TBC, TModel> FailResult<TBC, TModel>(
        string errorMessage)
        where TBC : IBoundedContext
        where TModel : IAnemicModel<TBC>
    {
        var defaultModel = default(TModel)!;
        var data = BusinessOperationData<TBC, TModel>
            .Commit<TBC, TModel>(defaultModel, defaultModel);
        return new AggregateResultException<TBC, TModel>(
            data, new FailedSpecification<TBC, TModel>(errorMessage));
    }
}
