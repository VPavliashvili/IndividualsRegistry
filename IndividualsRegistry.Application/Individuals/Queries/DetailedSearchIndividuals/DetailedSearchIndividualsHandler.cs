using AutoMapper;
using IndividualsRegistry.Application.Specifications;
using IndividualsRegistry.Domain.Contracts;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.DetailedSearchIndividuals;

public sealed class DetailedSearchIndividualsHandler
    : IRequestHandler<DetailedSearchIndividualsQuery, List<DetailedSearchIndividualsResponse>>
{
    private readonly IIndividualsRepository _repository;
    private readonly IMapper _mapper;

    public DetailedSearchIndividualsHandler(IIndividualsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<DetailedSearchIndividualsResponse>> Handle(
        DetailedSearchIndividualsQuery request,
        CancellationToken cancellationToken
    )
    {
        var relations=await _repository.GetRelationshipsByType(request.RelationType);

        var filter = new DetailedSearchSpec(
            request.PageSize,
            request.PageNumber,
            request.Name,
            request.Surname,
            request.PersonalId,
            request.Gender,
            request.BirthDate,
            request.CityId,
            request.PhoneNumber,
            request.RelationType,
            relations
        );

        var resp = await _repository.GetIndividuals(filter);
        var result = _mapper.Map<List<DetailedSearchIndividualsResponse>>(resp);

        return result;
    }
}
