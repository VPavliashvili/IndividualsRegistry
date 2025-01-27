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
            throw new AlreadyExistsException(individualEntity.Id);
        }

        await _dbContext.Individuals.AddAsync(individualEntity);
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

    public async Task<IndividualEntity?> GetIndividual(int individualId)
    {
        var result = await _dbContext.Individuals.SingleOrDefaultAsync(x => x.Id == individualId);
        return result;
    }

    public async Task RemoveIndividual(int individualId)
    {
        var existing =
            await _dbContext.Individuals.FirstOrDefaultAsync(x => x.Id == individualId)
            ?? throw new DoesNotExistException(individualId);
        _dbContext.Individuals.Remove(existing);
    }

    public async Task AddRelatedIndividual(
        int individualId,
        IndividualEntity relatedIndividual,
        RelationType relationType
    )
    {
        var target =
            await GetIndividual(individualId) ?? throw new DoesNotExistException(individualId);

        var relatedExists = (await GetIndividual(relatedIndividual.Id)) is not null;
        if (!relatedExists)
        {
            throw new DoesNotExistException(relatedIndividual.Id);
        }

        if (target.Id == relatedIndividual.Id)
        {
            throw new InvalidOperationException();
        }

        if (target.RelatedIndividuals?.Any(x => x.Id == relatedIndividual.Id) == true)
        {
            throw new RelatedIndividualAlreadyExists(individualId, relatedIndividual.Id);
        }

        var relation = new RelationEntity
        {
            IndividualId = individualId,
            RelatedIndividualId = relatedIndividual.Id,
            RelationType = relationType,
        };

        await _dbContext.Relations.AddAsync(relation);
    }

    public async Task RemoveRelatedIndividual(int individualId, int relatedIndividualId)
    {
        var target =
            await GetIndividual(individualId) ?? throw new DoesNotExistException(individualId);
        var related =
            await GetIndividual(relatedIndividualId)
            ?? throw new DoesNotExistException(relatedIndividualId);

        var relation =
            await _dbContext.Relations.FirstOrDefaultAsync(x =>
                x.IndividualId == individualId && x.RelatedIndividualId == relatedIndividualId
            ) ?? throw new RelationDoesNotExistException(individualId, relatedIndividualId);

        _dbContext.Relations.Remove(relation);
    }

    public async Task SetPicture(int individualId, byte[] image)
    {
        var existing =
            await _dbContext.Individuals.FirstOrDefaultAsync(x => x.Id == individualId)
            ?? throw new DoesNotExistException(individualId);
        existing.Picture = image;
        _dbContext.Entry(existing).Property(x => x.Picture).IsModified = true;
    }

    public async Task UpdateIndividual(IndividualEntity updatedEntity)
    {
        ArgumentNullException.ThrowIfNull(updatedEntity);

        var existing =
            await _dbContext.Individuals.FirstOrDefaultAsync(x => x.Id == updatedEntity.Id)
            ?? throw new DoesNotExistException(updatedEntity.Id);

        var entry = _dbContext.Entry(existing);

        var properties = typeof(IndividualEntity).GetProperties();

        foreach (var property in properties)
        {
            if (
                property.Name == nameof(IndividualEntity.Id)
                || property.Name == nameof(IndividualEntity.RelatedIndividuals)
                || property.Name == nameof(IndividualEntity.Picture)
                || property.Name == nameof(IndividualEntity.PhoneNumbers)
            )
                continue;

            var value = property.GetValue(updatedEntity);
            if (value is null)
                continue;

            property.SetValue(existing, value);
            entry.Property(property.Name).IsModified = true;
        }
    }
}
