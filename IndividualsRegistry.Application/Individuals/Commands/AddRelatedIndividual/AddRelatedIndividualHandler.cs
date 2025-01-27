using AutoMapper;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.AddRelatedIndividual;

public sealed class AddRelatedIndividualHandler : IRequestHandler<AddRelatedIndividualCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddRelatedIndividualHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        AddRelatedIndividualCommand request,
        CancellationToken cancellationToken
    )
    {
        await _unitOfWork.IndividualsRepository.AddRelatedIndividual(
            request.individualId,
            request.relatedIndividualId,
            request.relationType
        );
        await _unitOfWork.SaveChanges();
    }
}
