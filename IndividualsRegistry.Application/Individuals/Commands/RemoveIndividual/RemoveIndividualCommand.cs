using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.RemoveIndividual;

public sealed record RemoveIndividualCommand(int individualId) : IRequest;
