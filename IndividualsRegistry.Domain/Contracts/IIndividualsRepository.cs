using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Enums;
using IndividualsRegistry.Domain.Specifications;

namespace IndividualsRegistry.Domain.Contracts;

public interface IIndividualsRepository
{
    Task<int> AddIndividual(IndividualEntity individualEntity);
    Task UpdateIndividual(IndividualEntity updatedEntity);
    Task SetPicture(int individualId, byte[] image);
    Task RemoveIndividual(int individualId);

    Task<IEnumerable<IndividualEntity>> GetIndividuals(IIndividualSpecification? filter = null);
    Task<IndividualEntity?> GetIndividual(int individualId);

    Task AddRelatedIndividual(
        int individualId,
        IndividualEntity relatedIndividual,
        RelationType relationType
    );
    Task RemoveRelatedIndividual(int individualId, int relatedIndividualId);
}
