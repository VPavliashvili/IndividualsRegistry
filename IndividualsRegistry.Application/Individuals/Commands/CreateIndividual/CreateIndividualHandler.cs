using AutoMapper;
using FluentValidation;
using IndividualsRegistry.Application.Validation;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.CreateIndividual;

public class CreateIndividualHandler : IRequestHandler<CreateIndividualCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateIndividualCommand> _validator;

    public CreateIndividualHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateIndividualCommand> validator
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<int> Handle(
        CreateIndividualCommand request,
        CancellationToken cancellationToken
    )
    {
        await _validator.ValidateAndCustomException(request);

        var entity = _mapper.Map<IndividualEntity>(request.request);

        await _unitOfWork.IndividualsRepository.AddIndividual(entity);
        await _unitOfWork.SaveChanges();
        return entity.Id;
    }
}
