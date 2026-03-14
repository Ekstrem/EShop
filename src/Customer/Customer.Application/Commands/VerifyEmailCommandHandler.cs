using Customer.Domain;
using Customer.Domain.Abstraction;
using Customer.Domain.Implementation;
using Customer.Domain.Specifications;
using Customer.DomainServices;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;

namespace Customer.Application.Commands;

public sealed class VerifyEmailCommandHandler
    : IRequestHandler<VerifyEmailCommand, AggregateResult<ICustomer, ICustomerAnemicModel>>
{
    private readonly AggregateProvider _provider;
    private readonly Notifier _notifier;

    public VerifyEmailCommandHandler(AggregateProvider provider, Notifier notifier)
    {
        _provider = provider;
        _notifier = notifier;
    }

    public async Task<AggregateResult<ICustomer, ICustomerAnemicModel>> Handle(
        VerifyEmailCommand request,
        CancellationToken ct)
    {
        var current = await _provider.GetByIdAsync(request.CustomerId, ct)
            ?? throw new InvalidOperationException("Customer not found.");

        var validator = IsUnverifiedValidator.CreateInstance();
        if (!validator.IsSatisfiedBy(current))
            throw new InvalidOperationException(validator.ErrorMessage);

        var result = CustomerAggregate.VerifyEmail(current);
        await _notifier.HandleAsync(result, ct);
        return result;
    }
}
