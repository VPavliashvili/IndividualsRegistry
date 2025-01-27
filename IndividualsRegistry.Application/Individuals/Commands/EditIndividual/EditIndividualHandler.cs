using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.EditIndividual;

public class EditIndividualHandler : IRequestHandler<EditIndividualCommand, int>
{
    public Task<int> Handle(EditIndividualCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

