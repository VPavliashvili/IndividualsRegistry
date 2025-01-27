using AutoMapper;
using FluentValidation;
using IndividualsRegistry.Application.Validation;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IndividualsRegistry.Application.Individuals.Commands.CreateIndividual;

public class CreateIndividualHandler : IRequestHandler<CreateIndividualCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateIndividualCommand> _validator;
    private readonly ILogger<CreateIndividualHandler> _logger;

    public CreateIndividualHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateIndividualCommand> validator,
        ILogger<CreateIndividualHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    public async Task<int> Handle(
        CreateIndividualCommand request,
        CancellationToken cancellationToken
    )
    {
        // just testing
        _logger.LogInformation("hello from {handler}", nameof(CreateIndividualHandler));
        await _validator.ValidateAndCustomException(request);

        var entity = _mapper.Map<IndividualEntity>(request.request);

        await _unitOfWork.IndividualsRepository.AddIndividual(entity);
        await _unitOfWork.SaveChanges();
        return entity.Id;
    }
}
