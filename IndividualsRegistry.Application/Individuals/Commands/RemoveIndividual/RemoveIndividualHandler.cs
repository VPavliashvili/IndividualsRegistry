using IndividualsRegistry.Domain.Contracts;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.RemoveIndividual;

public sealed class RemoveIndividualHandler : IRequestHandler<RemoveIndividualCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveIndividualHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveIndividualCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.IndividualsRepository.RemoveIndividual(request.individualId);
        await _unitOfWork.SaveChanges();
    }
}
