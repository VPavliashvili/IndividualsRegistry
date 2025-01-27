using IndividualsRegistry.Domain.Enums;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.AddRelatedIndividual;

public sealed record AddRelatedIndividualCommand(
    int individualId,
    int relatedIndividualId,
    RelationType relationType
) : IRequest;
