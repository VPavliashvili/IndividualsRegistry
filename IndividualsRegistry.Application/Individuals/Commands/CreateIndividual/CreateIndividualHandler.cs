using AutoMapper;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.CreateIndividual;

public class CreateIndividualHandler : IRequestHandler<CreateIndividualCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateIndividualHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(
        CreateIndividualCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = _mapper.Map<IndividualEntity>(request);
        try
        {
            var res = await _unitOfWork.IndividualsRepository.AddIndividual(entity);
            await _unitOfWork.SaveChanges();
            return res;
        }
        catch
        {
            await _unitOfWork.RollbackChanges();
            throw;
        }
    }
}
