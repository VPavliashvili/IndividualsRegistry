using IndividualsRegistry.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndividualsRegistry.Infrastructure.Data;

public class IndividualsDbContext : DbContext
{
    public DbSet<IndividualEntity> Individuals { get; set; }

    public IndividualsDbContext(DbContextOptions<IndividualsDbContext> options)
        : base(options) { }
}
