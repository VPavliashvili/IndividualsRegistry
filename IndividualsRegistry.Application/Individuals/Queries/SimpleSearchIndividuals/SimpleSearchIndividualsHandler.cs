using AutoMapper;
using IndividualsRegistry.Application.Contracts;
using IndividualsRegistry.Application.Specifications;
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
        var filter = new SimpleSearchSpec(
            request.PageSize,
            request.PageNumber,
            request.Name,
            request.Surname,
            request.Personalid
        );
        var resp = await _repository.GetIndividuals(filter);
        var result = _mapper.Map<List<SimpleSearchIndividualsResponse>>(resp);
        return result;
    }
}
