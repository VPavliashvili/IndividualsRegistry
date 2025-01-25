using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Enums;
using IndividualsRegistry.Domain.Specifications;
using IndividualsRegistry.Infrastructure.Data;

namespace IndividualsRegistry.Infrastructure.Repositories;

public class IndividualsRepository : IIndividualsRepository
{
    private readonly IndividualsDbContext _dbContext;

    public IndividualsRepository(IndividualsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddIndividual(IndividualEntity individualEntity)
    {
        await _dbContext.Individuals.AddAsync(individualEntity);
    }

    public Task AddRelatedIndividual(int individualId, IndividualEntity relatedIndividual, RelationType relationType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IndividualEntity>> GetAllIndividuals(int pageSize, int pageNumber, IndividualFilter? filter = null)
    {
        throw new NotImplementedException();
    }

    public Task<IndividualEntity?> GetIndividual(int individualId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveIndividual(int individualId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRelatedIndividual(int individualId, int relatedIndividualId)
    {
        throw new NotImplementedException();
    }

    public Task SetPicture(int individualId, byte[] image, string contentType)
    {
        throw new NotImplementedException();
    }

    public Task UpdateIndividual(IndividualEntity updatedEntity)
    {
        throw new NotImplementedException();
    }
}
