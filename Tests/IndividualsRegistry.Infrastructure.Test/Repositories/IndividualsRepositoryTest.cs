using System.Linq.Expressions;
using System.Net.Mime;
using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Exceptions;
using IndividualsRegistry.Infrastructure.Data;
using IndividualsRegistry.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit.Abstractions;

namespace IndividualsRegistry.Infrastructure.Test.Repositories;

public class IndividualsRepositoryTest
{
    private readonly ITestOutputHelper _output;

    private static DbContextOptions<IndividualsDbContext> DbContextOptions =>
        new DbContextOptionsBuilder<IndividualsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

    public IndividualsRepositoryTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Test_Successfully_Adds_A_Valid_Individual()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
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

    [Fact]
    public async Task Test_Throws_When_Adding_Null()
    {
        // Given
        var entity = default(IndividualEntity);

        using var context = new IndividualsDbContext(DbContextOptions);
        var sut = new IndividualsRepository(context);

        // When
        var ex = await Record.ExceptionAsync(async () => await sut.AddIndividual(entity!));

        // Then
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task Test_Throws_AlreadyExistException_When_Adding_Duplicated_Data()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        var sut = new IndividualsRepository(context);

        // When
        await sut.AddIndividual(entity);
        await context.SaveChangesAsync();

        var ex = await Record.ExceptionAsync(async () => await sut.AddIndividual(entity));

        // Then
        Assert.IsType<AlreadyExistsException>(ex);
    }

    [Fact]
    public async Task Test_Throws_When_DbContextError()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "badname",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var set = new Mock<DbSet<IndividualEntity>>();
        set.Setup(x => x.AddAsync(entity, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("invalid name"));

        var context = new Mock<IndividualsDbContext>(DbContextOptions);
        context.Setup(x => x.Individuals).Throws(new Exception("validation err"));

        var sut = new IndividualsRepository(context.Object);

        // When
        var ex = await Record.ExceptionAsync(async () => await sut.AddIndividual(entity));

        // Then
        Assert.IsType<Exception>(ex);
        Assert.Equal("validation err", ex.Message);
    }
}
