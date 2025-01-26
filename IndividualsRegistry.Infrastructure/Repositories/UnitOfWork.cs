using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Infrastructure.Data;

namespace IndividualsRegistry.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IndividualsDbContext _context;

    public IIndividualsRepository IndividualsRepository { get; }

    public UnitOfWork(IIndividualsRepository repository, IndividualsDbContext context)
    {
        IndividualsRepository = repository;
        _context = context;
    }

    public async Task RollbackChanges()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public async Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
    }
}
