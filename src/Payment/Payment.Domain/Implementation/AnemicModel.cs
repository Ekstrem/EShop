namespace Payment.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Payment.Domain.Abstraction;

internal sealed class AnemicModel : IPaymentAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IPaymentRoot Root { get; internal set; } = null!;
    public IReadOnlyList<ITransaction> Transactions { get; internal set; } = new List<ITransaction>();
    public decimal TotalRefunded => Transactions
        .Where(t => t.Type == "Refund" && t.Status == "Completed")
        .Sum(t => t.Amount);

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
