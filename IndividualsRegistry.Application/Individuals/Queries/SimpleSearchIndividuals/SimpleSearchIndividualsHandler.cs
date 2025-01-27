using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.SimpleSearchIndividuals;

public class SimpleSearchIndividualsHandler
    : IRequestHandler<SimpleSearchIndividualsQuery, SimpleSearchIndividualsResponse>
{
    // inject concrete specification in ctor

    public Task<SimpleSearchIndividualsResponse> Handle(
        SimpleSearchIndividualsQuery request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}

