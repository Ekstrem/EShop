using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Implementation;

/// <summary>
/// Immutable implementation of the return label value object.
/// </summary>
internal sealed class ReturnLabel : IReturnLabel
{
    public string LabelUrl { get; private set; } = string.Empty;
    public string Carrier { get; private set; } = string.Empty;

    private ReturnLabel() { }

    public static IReturnLabel CreateInstance(
        string labelUrl,
        string carrier)
        => new ReturnLabel
        {
            LabelUrl = labelUrl,
            Carrier = carrier
        };
}
