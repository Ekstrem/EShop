namespace Hive.SeedWorks.TacticalPatterns;

using Hive.SeedWorks.Result;

/// <summary>Marker interface for a bounded context.</summary>
public interface IBoundedContext { }

/// <summary>Describes bounded context metadata.</summary>
public interface IBoundedContextDescription
{
    string ContextName { get; }
    int MicroserviceVersion { get; }
}

/// <summary>Marker interface for value objects.</summary>
public interface IValueObject { }

/// <summary>Marker interface for aggregate root entities.</summary>
public interface IAggregateRoot<TBC> where TBC : IBoundedContext { }

/// <summary>Marker interface for anemic model (state container).</summary>
public interface IAnemicModel<TBC> where TBC : IBoundedContext
{
    Guid Id { get; }
}

/// <summary>Base class for anemic model implementations.</summary>
public abstract class AnemicModel<TBC> : IAnemicModel<TBC> where TBC : IBoundedContext
{
    public Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>Base class for aggregate implementations.</summary>
public abstract class Aggregate<TBC, TModel> : IAggregate<TBC, TModel>
    where TBC : IBoundedContext
    where TModel : IAnemicModel<TBC>
{
    public TModel Model { get; set; } = default!;

    protected Aggregate() { }
    protected Aggregate(TModel model) => Model = model;
}

/// <summary>Aggregate interface.</summary>
public interface IAggregate<TBC, TModel>
    where TBC : IBoundedContext
    where TModel : IAnemicModel<TBC>
{
}

/// <summary>Provides aggregate instances.</summary>
public interface IAggregateProvider<TBC, TModel>
    where TBC : IBoundedContext
    where TModel : IAnemicModel<TBC>
{
    IAggregate<TBC, TModel> Create(TModel model);
}

/// <summary>Repository for command-side persistence.</summary>
public interface IRepository<TBC, TModel>
    where TBC : IBoundedContext
    where TModel : IAnemicModel<TBC>
{
    Task<TModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task SaveAsync(AggregateResult<TBC, TModel> result, CancellationToken ct = default);
}

/// <summary>Validator for business rules / specifications.</summary>
public interface IBusinessOperationValidator<in T>
{
    bool IsSatisfiedBy(T model);
    string ErrorMessage { get; }
}

/// <summary>Validator with bounded context awareness.</summary>
public interface IBusinessOperationValidator<TBC, in TModel>
    where TBC : IBoundedContext
{
    bool IsSatisfiedBy(TModel model);
    string ErrorMessage { get; }
}

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
