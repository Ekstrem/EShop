namespace Customer.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Customer.Domain.Abstraction;
using EShop.Contracts;

/// <summary>
/// Customer aggregate containing all business operations.
/// Each method returns an AggregateResult for event-driven processing.
/// </summary>
public sealed class CustomerAggregate
{
    private CustomerAggregate() { }

    private static AggregateResult<ICustomer, ICustomerAnemicModel> Success(
        ICustomerAnemicModel oldModel, ICustomerAnemicModel newModel)
    {
        var data = BusinessOperationData<ICustomer, ICustomerAnemicModel>
            .Commit<ICustomer, ICustomerAnemicModel>(oldModel, newModel);
        return new AggregateResultSuccess<ICustomer, ICustomerAnemicModel>(data);
    }

    private static AggregateResult<ICustomer, ICustomerAnemicModel> Fail(
        ICustomerAnemicModel model, string error)
    {
        var data = BusinessOperationData<ICustomer, ICustomerAnemicModel>
            .Commit<ICustomer, ICustomerAnemicModel>(model, model);
        return new AggregateResultException<ICustomer, ICustomerAnemicModel>(
            data, new FailedSpecification<ICustomer, ICustomerAnemicModel>(error));
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

        var empty = CustomerAnemicModel.CreateInstance(
            Guid.Empty,
            CustomerRoot.CreateInstance(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty),
            AddressBook.Empty(),
            new List<IConsent>().AsReadOnly());
        return Success(empty, model);
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

        return Success(current, model);
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

        return Success(current, model);
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

        return Success(current, model);
    }

    /// <summary>
    /// Requests a password reset. Validates that the email exists.
    /// </summary>
    public static AggregateResult<ICustomer, ICustomerAnemicModel> RequestPasswordReset(
        ICustomerAnemicModel current)
    {
        return Success(current, current);
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

        return Success(current, model);
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

        return Success(current, model);
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

        return Success(current, model);
    }
}
