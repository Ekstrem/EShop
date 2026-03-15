using Customer.Domain;
using Customer.Domain.Abstraction;
using Customer.Domain.Implementation;
using Customer.Domain.Specifications;
using Customer.DomainServices;
using Customer.InternalContracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using MediatR;

namespace Customer.Application.Commands;

public sealed class RegisterCustomerCommandHandler
    : IRequestHandler<RegisterCustomerCommand, AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    private readonly Notifier _notifier;
    private readonly ICustomerQueryRepository _queryRepository;

    public RegisterCustomerCommandHandler(
        Notifier notifier,
        ICustomerQueryRepository queryRepository)
    {
        _notifier = notifier;
        _queryRepository = queryRepository;
    }

    public async Task<AggregateResult<ICustomer, ICustomerAnemicModel>> Handle(
        RegisterCustomerCommand request,
        CancellationToken ct)
    {
        var emailValidator = EmailNotTakenValidator.CreateInstance(
            email => _queryRepository.EmailExistsAsync(email, ct).GetAwaiter().GetResult());

        var result = CustomerAggregate.RegisterCustomer(
            request.Email,
            request.FirstName,
            request.LastName,
            request.PasswordHash);

        if (!emailValidator.IsSatisfiedBy(result.Model()))
            throw new InvalidOperationException(emailValidator.Reason);

        await _notifier.HandleAsync(result, ct);
        return result;
    }
}
