using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.DetailedSearchIndividuals;

public sealed class DetailedSearchIndividualsHandler
    : IRequestHandler<DetailedSearchIndividualsQuery, DetailedSearchIndividualsResponse>
{
    // inject concrete specification in ctor

    public Task<DetailedSearchIndividualsResponse> Handle(
        DetailedSearchIndividualsQuery request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}

