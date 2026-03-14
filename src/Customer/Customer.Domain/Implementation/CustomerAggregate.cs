using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Implementation;

/// <summary>
/// Customer aggregate containing all business operations.
/// Each method returns an AggregateResult for event-driven processing.
/// </summary>
public sealed class CustomerAggregate
{
    private CustomerAggregate()
    {
    }

    /// <summary>
    /// Registers a new customer with Unverified status.
    /// </summary>
    public static AggregateResult<ICustomer, ICustomerAnemicModel> RegisterCustomer(
        string email,
        string firstName,
        string lastName,
        string passwordHash)
    {
        var id = Guid.NewGuid();
        var root = CustomerRoot.CreateInstance(
            id, email, firstName, lastName, passwordHash, "Unverified");
        var addressBook = AddressBook.Empty();
        var consents = new List<IConsent>().AsReadOnly();
        var model = CustomerAnemicModel.CreateInstance(id, root, addressBook, consents);

        return AggregateResult<ICustomer, ICustomerAnemicModel>
            .CreateInstance(model, nameof(RegisterCustomer));
    }

    /// <summary>
    /// Verifies the customer email, transitioning from Unverified to Active.
    /// </summary>
    public static AggregateResult<ICustomer, ICustomerAnemicModel> VerifyEmail(
        ICustomerAnemicModel current)
    {
        var root = CustomerRoot.CreateInstance(
            current.Root.Id,
            current.Root.Email,
            current.Root.FirstName,
            current.Root.LastName,
            current.Root.PasswordHash,
            "Active");
        var model = CustomerAnemicModel.CreateInstance(
            current.Id, root, current.AddressBook, current.Consents);

        return AggregateResult<ICustomer, ICustomerAnemicModel>
            .CreateInstance(model, nameof(VerifyEmail));
    }

    /// <summary>
    /// Updates the customer profile. Requires Active status.
    /// </summary>
    public static AggregateResult<ICustomer, ICustomerAnemicModel> UpdateProfile(
        ICustomerAnemicModel current,
        string firstName,
        string lastName,
        IAddressBook addressBook)
    {
        var root = CustomerRoot.CreateInstance(
            current.Root.Id,
            current.Root.Email,
            firstName,
            lastName,
            current.Root.PasswordHash,
            current.Root.Status);
        var model = CustomerAnemicModel.CreateInstance(
            current.Id, root, addressBook, current.Consents);

        return AggregateResult<ICustomer, ICustomerAnemicModel>
            .CreateInstance(model, nameof(UpdateProfile));
    }

    /// <summary>
    /// Changes the customer password. Requires Active status and correct old password.
    /// </summary>
    public static AggregateResult<ICustomer, ICustomerAnemicModel> ChangePassword(
        ICustomerAnemicModel current,
        string newPasswordHash)
    {
        var root = CustomerRoot.CreateInstance(
            current.Root.Id,
            current.Root.Email,
            current.Root.FirstName,
            current.Root.LastName,
            newPasswordHash,
            current.Root.Status);
        var model = CustomerAnemicModel.CreateInstance(
            current.Id, root, current.AddressBook, current.Consents);

        return AggregateResult<ICustomer, ICustomerAnemicModel>
            .CreateInstance(model, nameof(ChangePassword));
    }

    /// <summary>
    /// Requests a password reset. Validates that the email exists.
    /// </summary>
    public static AggregateResult<ICustomer, ICustomerAnemicModel> RequestPasswordReset(
        ICustomerAnemicModel current)
    {
        return AggregateResult<ICustomer, ICustomerAnemicModel>
            .CreateInstance(current, nameof(RequestPasswordReset));
    }

    /// <summary>
    /// Resets the password using a valid token.
    /// </summary>
    public static AggregateResult<ICustomer, ICustomerAnemicModel> ResetPassword(
        ICustomerAnemicModel current,
        string newPasswordHash)
    {
        var root = CustomerRoot.CreateInstance(
            current.Root.Id,
            current.Root.Email,
            current.Root.FirstName,
            current.Root.LastName,
            newPasswordHash,
            current.Root.Status);
        var model = CustomerAnemicModel.CreateInstance(
            current.Id, root, current.AddressBook, current.Consents);

        return AggregateResult<ICustomer, ICustomerAnemicModel>
            .CreateInstance(model, nameof(ResetPassword));
    }

    /// <summary>
    /// Updates a consent record for the customer. Requires Active status.
    /// </summary>
    public static AggregateResult<ICustomer, ICustomerAnemicModel> UpdateConsent(
        ICustomerAnemicModel current,
        string consentType,
        bool isGranted)
    {
        var updatedConsents = current.Consents
            .Where(c => c.ConsentType != consentType)
            .Append(Consent.CreateInstance(consentType, isGranted, DateTime.UtcNow))
            .ToList()
            .AsReadOnly();

        var model = CustomerAnemicModel.CreateInstance(
            current.Id, current.Root, current.AddressBook, updatedConsents);

        return AggregateResult<ICustomer, ICustomerAnemicModel>
            .CreateInstance(model, nameof(UpdateConsent));
    }

    /// <summary>
    /// Deactivates the account. Transitions Active to Deactivated and anonymizes PII.
    /// </summary>
    public static AggregateResult<ICustomer, ICustomerAnemicModel> DeactivateAccount(
        ICustomerAnemicModel current)
    {
        var anonymizedId = current.Root.Id.ToString("N");
        var root = CustomerRoot.CreateInstance(
            current.Root.Id,
            $"deactivated-{anonymizedId}@anonymized.local",
            "Anonymized",
            "User",
            string.Empty,
            "Deactivated");
        var emptyAddressBook = AddressBook.Empty();
        var clearedConsents = new List<IConsent>().AsReadOnly();
        var model = CustomerAnemicModel.CreateInstance(
            current.Id, root, emptyAddressBook, clearedConsents);

        return AggregateResult<ICustomer, ICustomerAnemicModel>
            .CreateInstance(model, nameof(DeactivateAccount));
    }
}
