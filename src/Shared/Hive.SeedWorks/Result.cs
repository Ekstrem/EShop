namespace Hive.SeedWorks.Result;

using Hive.SeedWorks.TacticalPatterns;

/// <summary>Result of an aggregate business operation.</summary>
public class AggregateResult<TBC, TModel>
    where TBC : IBoundedContext
    where TModel : IAnemicModel<TBC>
{
    public TModel? Model { get; private set; }
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string? EventName { get; private set; }

    private AggregateResult() { }

    /// <summary>Creates a successful result with the given model.</summary>
    public static AggregateResult<TBC, TModel> Ok(TModel model) => new()
    {
        Model = model,
        IsSuccess = true
    };

    /// <summary>Creates a successful result with an event name.</summary>
    public static AggregateResult<TBC, TModel> Create(TModel model, string eventName) => new()
    {
        Model = model,
        IsSuccess = true,
        EventName = eventName
    };

    /// <summary>Creates a failed result.</summary>
    public static AggregateResult<TBC, TModel> Fail(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}
