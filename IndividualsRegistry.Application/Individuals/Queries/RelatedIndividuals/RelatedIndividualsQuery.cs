using IndividualsRegistry.Domain.Enums;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.RelatedIndividuals;

public sealed record RelatedIndividualsQuery(RelationType relationType)
    : IRequest<RelatedIndividualsResponse>;
