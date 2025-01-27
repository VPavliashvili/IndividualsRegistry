using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.SetPicture;

public sealed class SetPictureHandler : IRequestHandler<SetPictureCommand, int>
{
    public Task<int> Handle(SetPictureCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

