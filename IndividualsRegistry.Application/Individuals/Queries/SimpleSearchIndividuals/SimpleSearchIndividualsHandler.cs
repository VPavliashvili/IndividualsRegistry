using AutoMapper;
using IndividualsRegistry.Domain.Contracts;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.SimpleSearchIndividuals;

public class SimpleSearchIndividualsHandler
    : IRequestHandler<SimpleSearchIndividualsQuery, List<SimpleSearchIndividualsResponse>>
{
    private readonly IIndividualsRepository _repository;
    private readonly IMapper _mapper;

    public SimpleSearchIndividualsHandler(IIndividualsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<SimpleSearchIndividualsResponse>> Handle(
        SimpleSearchIndividualsQuery request,
        CancellationToken cancellationToken
    )
    {
        var resp = await _repository.GetIndividuals(request.Filter);
        var result = _mapper.Map<List<SimpleSearchIndividualsResponse>>(resp);
        return result;
    }
}
