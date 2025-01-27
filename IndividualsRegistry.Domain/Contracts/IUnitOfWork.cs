namespace IndividualsRegistry.Domain.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChanges();
    IIndividualsRepository IndividualsRepository { get; }
}
