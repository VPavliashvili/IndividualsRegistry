namespace IndividualsRegistry.Application.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChanges();
    IIndividualsRepository IndividualsRepository { get; }
}
