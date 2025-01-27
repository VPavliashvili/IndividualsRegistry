using AutoMapper;
using FluentValidation;
using IndividualsRegistry.Application.Validation;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.EditIndividual;

public class EditIndividualHandler : IRequestHandler<EditIndividualCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<EditIndividualCommand> _validator;

    public EditIndividualHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<EditIndividualCommand> validator
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task Handle(EditIndividualCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndCustomException(request);

        var entity = _mapper.Map<IndividualEntity>(request);

        await _unitOfWork.IndividualsRepository.UpdateIndividual(entity);
        await _unitOfWork.SaveChanges();
    }
}
