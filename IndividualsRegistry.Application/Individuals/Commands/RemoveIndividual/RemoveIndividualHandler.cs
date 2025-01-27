using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.RemoveIndividual;

public sealed class RemoveIndividualHandler : IRequestHandler<RemoveIndividualCommand, int>
{
    public Task<int> Handle(RemoveIndividualCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

