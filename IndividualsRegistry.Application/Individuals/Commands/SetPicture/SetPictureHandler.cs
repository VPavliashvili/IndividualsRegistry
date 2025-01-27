using IndividualsRegistry.Application.Contracts;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.SetPicture;

public sealed class SetPictureHandler : IRequestHandler<SetPictureCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public SetPictureHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SetPictureCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.IndividualsRepository.SetPicture(request.individualId, request.image);
        await _unitOfWork.SaveChanges();
    }
}
