using IndividualsRegistry.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndividualsRegistry.Infrastructure.Data;

public class IndividualsDbContext : DbContext
{
    public virtual DbSet<IndividualEntity> Individuals { get; set; }
    public virtual DbSet<RelationEntity> Relations { get; set; }

    public IndividualsDbContext(DbContextOptions<IndividualsDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IndividualEntity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity
                .HasMany(e => e.RelatedIndividuals)
                .WithMany()
                .UsingEntity<RelationEntity>(
                    j =>
                        j.HasOne<IndividualEntity>()
                            .WithMany()
                            .HasForeignKey(x => x.RelatedIndividualId),
                    j => j.HasOne<IndividualEntity>().WithMany().HasForeignKey(x => x.IndividualId),
                    j =>
                    {
                        j.HasKey(x => new { x.IndividualId, x.RelatedIndividualId });
                        j.Property(x => x.RelationType).IsRequired();
                    }
                );
        });
    }
}
