using IndividualsRegistry.Application.Specifications;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.SimpleSearchIndividuals;

public class SimpleSearchIndividualsQuery : IRequest<List<SimpleSearchIndividualsResponse>>
{
    public SimpleSearchSpec Filter { get; }

    public SimpleSearchIndividualsQuery(SimpleSearchSpec spec)
    {
        Filter = spec;
    }
}
