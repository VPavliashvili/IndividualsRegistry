using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.AddRelatedIndividual;

public sealed record AddRelatedIndividualCommand(AddRelatedIndividualRequest request)
    : IRequest<int>;
