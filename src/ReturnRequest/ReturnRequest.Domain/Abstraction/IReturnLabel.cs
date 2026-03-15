using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace ReturnRequest.Domain.Abstraction;

/// <summary>
/// Value object representing a return shipping label.
/// </summary>
public interface IReturnLabel : IValueObject
{
    string LabelUrl { get; }
    string Carrier { get; }
}
