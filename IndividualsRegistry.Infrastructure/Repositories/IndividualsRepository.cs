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
            return await _dbContext
                .Individuals.Include(x => x.PhoneNumbers)
                .Include(x => x.RelatedIndividuals)
                .ToListAsync();
        }

        var pageSize = filter.PageSize;
        var pageNumber = filter.PageNumber;

        var intermediary = _dbContext
            .Individuals.Include(x => x.PhoneNumbers)
            .Include(x => x.RelatedIndividuals)
            .AsQueryable();
        if (filter.Criteria is not null)
        {
            intermediary = intermediary.Where(filter.Criteria);
        }

        var skipped = (pageNumber - 1) * pageSize;
        var count = intermediary.Count();
        if (skipped >= count && count > 0)
        {
            var maxpage = count / pageSize + count % pageSize != 0 ? 1 : 0;
            throw new PageNumberOverflowException(maxpage, pageNumber, count);
        }

        var result = await intermediary.Skip(skipped).Take(pageSize).ToListAsync();
        return result;
    }

    public async Task<IndividualEntity?> GetIndividual(int individualId)
    {
        var result = await _dbContext
            .Individuals.Include(x => x.PhoneNumbers)
            .Include(x => x.RelatedIndividuals)
            .SingleOrDefaultAsync(x => x.Id == individualId);
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
        int relatedIndividualId,
        RelationType relationType
    )
    {
        var target =
            await GetIndividual(individualId) ?? throw new DoesNotExistException(individualId);

        var relatedExists = (await GetIndividual(relatedIndividualId)) is not null;
        if (!relatedExists)
        {
            throw new DoesNotExistException(relatedIndividualId);
        }

        if (target.Id == relatedIndividualId)
        {
            throw new InvalidOperationException();
        }

        if (target.RelatedIndividuals?.Any(x => x.Id == relatedIndividualId) == true)
        {
            throw new RelatedIndividualAlreadyExists(individualId, relatedIndividualId);
        }

        var relation = new RelationEntity
        {
            IndividualId = individualId,
            RelatedIndividualId = relatedIndividualId,
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

    public async Task<IEnumerable<RelationEntity>> GetRelationshipsByType(RelationType? type)
    {
        if (type is null)
            return [];

        var result = await _dbContext.Relations.Where(x => x.RelationType == type).ToListAsync();
        return result;
    }

    public async Task<Dictionary<string, int>> GroupRelationsByTypeAndIndividual(int individualId)
    {
        var existing =
            await GetIndividual(individualId) ?? throw new DoesNotExistException(individualId);

        var res = await _dbContext
            .Relations.Where(x => x.IndividualId == individualId)
            .GroupBy(x => x.RelationType)
            .ToDictionaryAsync(g => g.Key.ToString(), g => g.Count());

        return res;
    }
}
