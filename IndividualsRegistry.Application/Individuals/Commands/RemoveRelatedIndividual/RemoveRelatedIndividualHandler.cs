using IndividualsRegistry.Application.Contracts;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.RemoveRelatedIndividual;

public sealed class RemoveRelatedIndividualHandler : IRequestHandler<RemoveRelatedIndividualCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRelatedIndividualHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        RemoveRelatedIndividualCommand request,
        CancellationToken cancellationToken
    )
    {
        await _unitOfWork.IndividualsRepository.RemoveRelatedIndividual(
            request.individualId,
            request.relatedIndividualid
        );
        await _unitOfWork.SaveChanges();
    }
}
