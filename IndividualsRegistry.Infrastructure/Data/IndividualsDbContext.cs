using System.Data;
using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IndividualsRegistry.Infrastructure.Data;

public class IndividualsDbContext : DbContext
{
    public virtual DbSet<IndividualEntity> Individuals { get; set; }
    public virtual DbSet<RelationEntity> Relations { get; set; }
    public virtual DbSet<PhoneNumberEntity> PhoneNumbers { get; set; }

    public IndividualsDbContext(DbContextOptions<IndividualsDbContext> options)
        : base(options) { }

    public IndividualsDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IndividualEntity>(entity =>
        {
            entity.ToTable(
                nameof(Individuals),
                tb =>
                {
                    tb.HasCheckConstraint("CK_Name_Length", "LEN(Name) >= 2 AND LEN(Name) <= 50");
                    tb.HasCheckConstraint("CK_Name_Characters", "Name NOT LIKE '%[^a-zA-Zა-ჰ]%'");

                    tb.HasCheckConstraint(
                        "CK_Surname_Length",
                        "LEN(Surname) >= 2 AND LEN(Surname) <= 50"
                    );
                    tb.HasCheckConstraint(
                        "CK_Surname_Characters",
                        "Surname NOT LIKE '%[^a-zA-Zა-ჰ]%'"
                    );

                    tb.HasCheckConstraint(
                        "CK_Gender",
                        $"Gender IN ('{nameof(Gender.Male)}', '{nameof(Gender.Female)}')"
                    );

                    tb.HasCheckConstraint(
                        "CK_PersonalId",
                        "PersonalId LIKE "
                            + "'[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'"
                    );

                    tb.HasCheckConstraint(
                        "CK_Individual_MinimumAge",
                        "DATEDIFF(YEAR, BirthDate, GETDATE()) >= 18"
                    );
                }
            );

            entity
                .HasIndex(e => e.PersonalId)
                .IsUnique()
                .HasDatabaseName("UX_Individual_PersonalId");

            entity.HasKey(x => x.Id);
            entity
                .Property(x => x.Id)
                .IsRequired()
                .HasColumnType(SqlDbType.Int.ToString())
                .ValueGeneratedOnAdd();

            entity
                .Property(x => x.Name)
                .IsRequired()
                .HasColumnType(SqlDbType.NVarChar.ToString())
                .HasMaxLength(10);
            entity
                .Property(x => x.Surname)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnType(SqlDbType.NVarChar.ToString());
            entity
                .Property(x => x.Gender)
                .IsRequired()
                .HasColumnType(SqlDbType.NVarChar.ToString())
                .HasMaxLength(10)
                .HasConversion<string>();
            entity
                .Property(x => x.PersonalId)
                .IsFixedLength()
                .HasMaxLength(11)
                .IsRequired()
                .HasColumnType(SqlDbType.NVarChar.ToString());
            entity
                .Property(x => x.BirthDate)
                .IsRequired()
                .HasColumnType(SqlDbType.Date.ToString())
                .HasConversion<DateOnly>();
            entity.Property(x => x.CityId).HasColumnType(SqlDbType.Int.ToString());
            entity
                .HasMany(e => e.RelatedIndividuals)
                .WithMany()
                .UsingEntity<RelationEntity>(
                    j =>
                        j.HasOne<IndividualEntity>()
                            .WithMany()
                            .HasForeignKey(x => x.RelatedIndividualId)
                            .OnDelete(DeleteBehavior.ClientCascade),
                    j => j.HasOne<IndividualEntity>().WithMany().HasForeignKey(x => x.IndividualId),
                    j =>
                    {
                        j.HasKey(x => new { x.IndividualId, x.RelatedIndividualId });
                        j.Property(x => x.RelationType).IsRequired();
                    }
                );
        });

        modelBuilder.Entity<PhoneNumberEntity>(entity =>
        {
            entity.ToTable(
                nameof(PhoneNumbers),
                tb =>
                    tb.HasCheckConstraint(
                        "CK_PhoneNumberType",
                        $"Type IN ('{nameof(PhoneNumberType.Mobile)}', '{nameof(PhoneNumberType.Office)}', '{nameof(PhoneNumberType.Home)}')"
                    )
            );
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.Number).IsRequired().HasMaxLength(50);

            entity.Property(x => x.Type).HasConversion<string>().HasMaxLength(50);

            entity
                .HasOne(x => x.Individual)
                .WithMany(x => x.PhoneNumbers)
                .HasForeignKey(x => x.Individualid)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>().HaveColumnType(SqlDbType.Date.ToString());
    }
}
