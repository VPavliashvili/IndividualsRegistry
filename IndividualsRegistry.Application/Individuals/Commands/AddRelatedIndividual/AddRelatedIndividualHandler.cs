using FluentValidation;
using IndividualsRegistry.Application.Contracts;
using IndividualsRegistry.Application.Validation;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.AddRelatedIndividual;

public sealed class AddRelatedIndividualHandler : IRequestHandler<AddRelatedIndividualCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddRelatedIndividualCommand> _validator;

    public AddRelatedIndividualHandler(
        IUnitOfWork unitOfWork,
        IValidator<AddRelatedIndividualCommand> validator
    )
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task Handle(
        AddRelatedIndividualCommand request,
        CancellationToken cancellationToken
    )
    {
        await _validator.ValidateAndCustomException(request);

        await _unitOfWork.IndividualsRepository.AddRelatedIndividual(
            request.individualId,
            request.relatedIndividualId,
            request.relationType
        );
        await _unitOfWork.SaveChanges();
    }
}
