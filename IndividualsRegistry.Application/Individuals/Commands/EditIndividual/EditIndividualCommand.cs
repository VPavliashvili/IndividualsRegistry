using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.EditIndividual;

public sealed record EditIndividualCommand(EditIndividualRequest request) : IRequest<int>;
