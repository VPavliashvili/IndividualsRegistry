using AutoMapper;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.EditIndividual;

public class EditIndividualHandler : IRequestHandler<EditIndividualCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditIndividualHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(EditIndividualCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<IndividualEntity>(request);

        await _unitOfWork.IndividualsRepository.UpdateIndividual(entity);
        await _unitOfWork.SaveChanges();
    }
}
