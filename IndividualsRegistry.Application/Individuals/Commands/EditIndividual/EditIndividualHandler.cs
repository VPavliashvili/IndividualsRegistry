using FluentValidation;
using IndividualsRegistry.Application.Contracts;
using IndividualsRegistry.Application.Validation;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.EditIndividual;

public class EditIndividualHandler : IRequestHandler<EditIndividualCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<EditIndividualCommand> _validator;

    public EditIndividualHandler(
        IUnitOfWork unitOfWork,
        IValidator<EditIndividualCommand> validator
    )
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task Handle(EditIndividualCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndCustomException(request);

        await _unitOfWork.IndividualsRepository.UpdateIndividual(request);
        await _unitOfWork.SaveChanges();
    }
}
