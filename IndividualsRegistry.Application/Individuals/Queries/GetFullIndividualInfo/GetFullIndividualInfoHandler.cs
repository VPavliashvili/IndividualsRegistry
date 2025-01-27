using AutoMapper;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Exceptions;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.GetFullIndividualInfo;

public sealed class GetFullIndividualInfoHandler
    : IRequestHandler<GetFullIndividualInfoQuery, GetFullIndividualInfoResponse>
{
    private readonly IIndividualsRepository _repository;
    private readonly IMapper _mapper;

    public GetFullIndividualInfoHandler(IIndividualsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetFullIndividualInfoResponse> Handle(
        GetFullIndividualInfoQuery request,
        CancellationToken cancellationToken
    )
    {
        var resp =
            await _repository.GetIndividual(request.IndividualId)
            ?? throw new DoesNotExistException(request.IndividualId);
        var result = _mapper.Map<GetFullIndividualInfoResponse>(resp);

        return result;
    }
}
