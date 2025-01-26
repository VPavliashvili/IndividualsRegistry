namespace IndividualsRegistry.Domain.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChanges();
    Task RollbackChanges();
    IIndividualsRepository IndividualsRepository { get; }
}

