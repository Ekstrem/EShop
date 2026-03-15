using Customer.Domain;
using Customer.Domain.Abstraction;
using Customer.Domain.Implementation;
using Customer.Domain.Specifications;
using Customer.DomainServices;
using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;

namespace Customer.Application.Commands;

public sealed class UpdateProfileCommandHandler
    : IRequestHandler<UpdateProfileCommand, AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly Notifier _notifier;

    public UpdateProfileCommandHandler(AggregateProvider provider, Notifier notifier)
    {
        _provider = provider;
        _notifier = notifier;
    }

    public async Task<AggregateResult<ICustomer, ICustomerAnemicModel>> Handle(
        UpdateProfileCommand request,
        CancellationToken ct)
    {
        var current = await _provider.GetByIdAsync(request.CustomerId, ct)
            ?? throw new InvalidOperationException("Customer not found.");

        var activeValidator = IsActiveValidator.CreateInstance();
        if (!activeValidator.IsSatisfiedBy(current))
            throw new InvalidOperationException(activeValidator.ErrorMessage);

        var addresses = request.Addresses
            .Select(a => (IAddress)Address.CreateInstance(
                a.Street, a.City, a.ZipCode, a.Country, a.IsDefault))
            .ToList()
            .AsReadOnly();
        var addressBook = AddressBook.CreateInstance(addresses);

        var addressLimitValidator = AddressBookLimitValidator.CreateInstance();
        var tempModel = CustomerAnemicModel.CreateInstance(
            current.Id, current.Root, addressBook, current.Consents);
        if (!addressLimitValidator.IsSatisfiedBy(tempModel))
            throw new InvalidOperationException(addressLimitValidator.ErrorMessage);

        var result = CustomerAggregate.UpdateProfile(
            current, request.FirstName, request.LastName, addressBook);
        await _notifier.HandleAsync(result, ct);
        return result;
    }
}
