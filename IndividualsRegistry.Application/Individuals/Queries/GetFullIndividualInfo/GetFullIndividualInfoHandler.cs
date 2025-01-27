using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.GetFullIndividualInfo;

public sealed class GetFullIndividualInfoHandler
    : IRequestHandler<GetFullIndividualInfoQuery, GetFullIndividualInfoResponse>
{
    public Task<GetFullIndividualInfoResponse> Handle(
        GetFullIndividualInfoQuery request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}

