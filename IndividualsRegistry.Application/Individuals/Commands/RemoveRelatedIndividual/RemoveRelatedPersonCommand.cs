using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.RemoveRelatedIndividual;

public sealed record RemoveRelatedIndividualCommand(int individualId, int relatedIndividualid)
    : IRequest;
