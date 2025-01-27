using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.RelatedIndividuals;

public sealed record RelatedIndividualsQuery() : IRequest<IEnumerable<RelatedIndividualsResponse>>;
