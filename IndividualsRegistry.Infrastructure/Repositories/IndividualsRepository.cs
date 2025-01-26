using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Enums;
using IndividualsRegistry.Domain.Exceptions;
using IndividualsRegistry.Domain.Specifications;
using IndividualsRegistry.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
        ArgumentNullException.ThrowIfNull(individualEntity);

        var alreadyExists = await _dbContext.Individuals.AnyAsync(x => x.Id == individualEntity.Id);
        if (alreadyExists)
        {
            throw new AlreadyExistsException();
        }

        await _dbContext.Individuals.AddAsync(individualEntity);
    }

    public Task AddRelatedIndividual(
        int individualId,
        IndividualEntity relatedIndividual,
        RelationType relationType
    )
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IndividualEntity>> GetIndividuals(
        IIndividualSpecification? filter = null
    )
    {
        if (filter is null)
        {
            return await _dbContext.Individuals.ToListAsync();
        }

        var pageSize = filter.PageSize ?? int.MaxValue;
        var pageNumber = filter.PageNumber ?? 1;

        var intermediary = _dbContext.Individuals.AsQueryable();
        if (filter.Criteria is not null)
        {
            intermediary = intermediary.Where(filter.Criteria);
        }

        var result = await intermediary
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return result;
    }

    public Task<IndividualEntity?> GetIndividual(int individualId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveIndividual(int individualId)
    {
        var existing =
            await _dbContext.Individuals.FirstOrDefaultAsync(x => x.Id == individualId)
            ?? throw new DoesNotExistException();
        _dbContext.Individuals.Remove(existing);
    }

    public Task RemoveRelatedIndividual(int individualId, int relatedIndividualId)
    {
        throw new NotImplementedException();
    }

    public async Task SetPicture(int individualId, byte[] image)
    {
        var existing =
            await _dbContext.Individuals.FirstOrDefaultAsync(x => x.Id == individualId)
            ?? throw new DoesNotExistException();
        existing.Picture = image;
        _dbContext.Entry(existing).Property(x => x.Picture).IsModified = true;
    }

    public async Task UpdateIndividual(IndividualEntity updatedEntity)
    {
        ArgumentNullException.ThrowIfNull(updatedEntity);

        var existing =
            await _dbContext.Individuals.FirstOrDefaultAsync(x => x.Id == updatedEntity.Id)
            ?? throw new DoesNotExistException();
        _dbContext.Entry(existing).CurrentValues.SetValues(updatedEntity);
    }
}
