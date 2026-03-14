namespace Customer.InternalContracts;

/// <summary>
/// Read model projection for customer queries.
/// </summary>
public sealed class CustomerReadModel
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public List<AddressReadModel> Addresses { get; init; } = new();
    public List<ConsentReadModel> Consents { get; init; } = new();
}

public sealed class AddressReadModel
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public bool IsDefault { get; init; }
}

public sealed class ConsentReadModel
{
    public string ConsentType { get; init; } = string.Empty;
    public bool IsGranted { get; init; }
    public DateTime GrantedAt { get; init; }
}
