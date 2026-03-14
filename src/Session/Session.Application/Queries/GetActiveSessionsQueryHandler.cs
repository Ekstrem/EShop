using MediatR;
using Session.InternalContracts;

namespace Session.Application.Queries;

public sealed class GetActiveSessionsQueryHandler
    : IRequestHandler<GetActiveSessionsQuery, IReadOnlyList<SessionReadModel>>
{
    private readonly ISessionQueryRepository _queryRepository;

    public GetActiveSessionsQueryHandler(ISessionQueryRepository queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<IReadOnlyList<SessionReadModel>> Handle(
        GetActiveSessionsQuery request,
        CancellationToken ct)
    {
        return await _queryRepository.GetActiveByCustomerIdAsync(request.CustomerId, ct);
    }
}
