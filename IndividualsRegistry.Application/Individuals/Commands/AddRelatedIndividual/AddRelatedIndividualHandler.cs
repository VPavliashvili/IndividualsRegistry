using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.AddRelatedIndividual;

public sealed class AddRelatedIndividualHandler : IRequestHandler<AddRelatedIndividualCommand, int>
{
    public Task<int> Handle(
        AddRelatedIndividualCommand request,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}

