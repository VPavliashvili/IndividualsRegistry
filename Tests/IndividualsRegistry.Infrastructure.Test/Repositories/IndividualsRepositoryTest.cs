using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Infrastructure.Data;
using IndividualsRegistry.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IndividualsRegistry.Infrastructure.Test.Repositories;

public class IndividualsRepositoryTest
{
    [Fact]
    public async void Successfully_Adds_A_Valid_Individual()
    {
        // Given
        var entity = new IndividualEntity()
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        var options = new DbContextOptionsBuilder<IndividualsDbContext>().UseInMemoryDatabase(databaseName: "test").Options;

        using var context = new IndividualsDbContext(options);
        var sut = new IndividualsRepository(context);

        // When
        await sut.AddIndividual(entity);
        await context.SaveChangesAsync();

        // Then
        var entry = await context.Set<IndividualEntity>().FirstOrDefaultAsync();
        Assert.NotNull(entry);
        Assert.Equal(entity.Name, entry.Name);
        Assert.Equal(entity.BirthDate, entry.BirthDate);
        Assert.Equal(entity.Gender, entry.Gender);
        Assert.Equal(entity.City, entry.City);
    }
}
