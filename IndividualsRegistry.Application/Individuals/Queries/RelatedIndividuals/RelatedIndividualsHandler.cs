using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.RelatedIndividuals;

public sealed class RelatedIndividualsHandler
    : IRequestHandler<RelatedIndividualsQuery, RelatedIndividualsResponse>
{
    public Task<RelatedIndividualsResponse> Handle(
        RelatedIndividualsQuery request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}

