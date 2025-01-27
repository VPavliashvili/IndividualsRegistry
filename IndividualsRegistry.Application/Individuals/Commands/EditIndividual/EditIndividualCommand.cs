using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.EditIndividual;

public sealed record EditIndividualCommand(int individualId, EditIndividualRequest request)
    : IRequest;
