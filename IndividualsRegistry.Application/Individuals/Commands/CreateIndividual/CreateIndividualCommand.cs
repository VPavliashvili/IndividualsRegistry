using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.CreateIndividual;

public sealed record CreateIndividualCommand(CreateIndividualRequest request) : IRequest<int>;
