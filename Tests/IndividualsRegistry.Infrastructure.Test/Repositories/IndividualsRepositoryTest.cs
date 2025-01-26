using System.Linq.Expressions;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Exceptions;
using IndividualsRegistry.Domain.Specifications;
using IndividualsRegistry.Infrastructure.Data;
using IndividualsRegistry.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        var context = new Mock<IndividualsDbContext>(DbContextOptions);
        context.Setup(x => x.Individuals).Throws(new Exception("validation err"));

        var sut = new IndividualsRepository(context.Object);

        // When
        var ex = await Record.ExceptionAsync(async () => await sut.AddIndividual(entity));

        // Then
        Assert.IsType<Exception>(ex);
        Assert.Equal("validation err", ex.Message);
    }

    [Fact]
    public async Task Test_Successfully_Updates_Existing_Individual()
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
        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        var updated = new IndividualEntity
        {
            Id = 1,
            Name = "newName",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        var sut = new IndividualsRepository(context);
        // When
        await sut.UpdateIndividual(updated);
        await context.SaveChangesAsync();

        // Then
        var resutl = await context.Individuals.FindAsync(1);
        _output.WriteLine(resutl!.Name);
        Assert.NotNull(resutl);
        Assert.Equal("newName", resutl.Name);
    }

    [Fact]
    public async Task Test_Throws_When_Updating_Individual_Does_Not_Exist()
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
        var context = new Mock<IndividualsDbContext>(DbContextOptions);
        context.Setup(x => x.Individuals).ReturnsDbSet([]);

        var sut = new IndividualsRepository(context.Object);
        // When
        var ex = await Record.ExceptionAsync(async () => await sut.UpdateIndividual(entity));

        // Then
        Assert.IsType<DoesNotExistException>(ex);
    }

    [Fact]
    public async Task Test_Throws_When_Update_With_Null_Entity()
    {
        // Given
        var context = new Mock<IndividualsDbContext>(DbContextOptions);

        var sut = new IndividualsRepository(context.Object);
        // When
        var ex = await Record.ExceptionAsync(async () => await sut.UpdateIndividual(default!));
        // Then
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public async Task Test_Update_Should_Not_Modify_Other_Records()
    {
        // Given
        var entity1 = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var entity2 = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "10987654321",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entity1, entity2);
        await context.SaveChangesAsync();

        var updated = new IndividualEntity
        {
            Id = 1,
            Name = "peter",
            Surname = "parker",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        var sut = new IndividualsRepository(context);
        // When
        await sut.UpdateIndividual(updated);
        await context.SaveChangesAsync();

        // Then
        var unchanged = await context.Individuals.FindAsync(2);
        var changed = await context.Individuals.FindAsync(1);
        Assert.Equal("bruce", unchanged!.Name);
        Assert.Equal("peter", changed!.Name);
    }

    [Fact]
    public async Task Test_Successfully_SetsPicture_With_Valid_Data()
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
        await context.Individuals.AddAsync(entity);
        await context.SaveChangesAsync();

        var imageBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 };

        var sut = new IndividualsRepository(context);
        // When
        await sut.SetPicture(1, imageBytes);
        await context.SaveChangesAsync();

        // Then
        var res = await context.Individuals.FindAsync(1);
        Assert.NotNull(res!.Picture);
        Assert.Equal(imageBytes, res.Picture);
    }

    [Fact]
    public async Task Test_Throws_When_SetPicture_Individual_Does_Not_Exist()
    {
        // Given
        int wrongId = 123;
        using var context = new IndividualsDbContext(DbContextOptions);

        var sut = new IndividualsRepository(context);
        // When
        var ex = await Record.ExceptionAsync(async () => await sut.SetPicture(wrongId, []));

        // Then
        Assert.IsType<DoesNotExistException>(ex);
    }

    [Fact]
    public async Task Test_SetPicture_Successfully_Updates_Existing_Picture()
    {
        // Given
        var imageBytes = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 };
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
            Picture = imageBytes,
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddAsync(entity);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        var newImagebytes = new byte[] { 0xD8, 0xD8, 0xFF, 0xE0 };

        // When
        await sut.SetPicture(1, newImagebytes);
        await context.SaveChangesAsync();

        // Then
        var updated = await context.Individuals.FindAsync(1);
        Assert.Equal(newImagebytes, updated!.Picture);
    }

    [Fact]
    public async Task Test_RemoveIndividual_WithValidId_When_Successful()
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
        await context.Individuals.AddAsync(entity);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        await sut.RemoveIndividual(1);
        await context.SaveChangesAsync();

        // Then
        var result = await context.Individuals.FindAsync(1);
        Assert.Null(result);
    }

    [Fact]
    public async Task Test_Throws_When_RemoveIndividual_Does_Not_Exist()
    {
        // Given
        var wrongId = 123;

        using var context = new IndividualsDbContext(DbContextOptions);
        var sut = new IndividualsRepository(context);
        // When
        var ex = await Record.ExceptionAsync(async () => await sut.RemoveIndividual(wrongId));

        // Then
        Assert.IsType<DoesNotExistException>(ex);
    }

    [Fact]
    public async Task Test_RemoveIndividual_Should_Not_Affect_Other_Records()
    {
        // Given
        var entity1 = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var entity2 = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entity1, entity2);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        await sut.RemoveIndividual(2);

        // Then
        var remaining = await context.Individuals.FindAsync(1);
        Assert.NotNull(remaining);
        Assert.Equal("john", remaining.Name);
    }

    [Fact]
    public async Task Test_Removeindividual_WithRelatedPerson_ShouldRemoveRelationship()
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
        var related = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "12345678901",
            Gender = "man",
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entity, related);
        await context.AddAsync(
            new RelationEntity
            {
                IndividualId = 1,
                RelatedIndividualId = 2,
                RelationType = Domain.Enums.RelationType.Other,
            }
        );
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        await sut.RemoveIndividual(1);
        await context.SaveChangesAsync();

        // Then
        var relations = await context.Relations.ToListAsync();
        Assert.Empty(relations);
    }

    [Fact]
    public async Task Test_GetIndividuals_WithoutSpecification_ShouldReturnAll()
    {
        // Given
        var individuals = new[]
        {
            new IndividualEntity
            {
                Id = 1,
                Name = "john",
                Surname = "doe",
                PersonalId = "12345678901",
                Gender = "man",
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
            new IndividualEntity
            {
                Id = 2,
                Name = "bruce",
                Surname = "wayne",
                PersonalId = "12345678901",
                Gender = "man",
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
        };
        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(individuals);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        var result = await sut.GetIndividuals();

        // Then
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Test_GetAllIndividuals_WithPagination_ShouldReturnCorrectPage()
    {
        // Given
        var entities = Enumerable
            .Range(1, 15)
            .Select(i => new IndividualEntity
            {
                Id = i,
                Name = $"name{i}",
                Surname = "doe",
                PersonalId = "12345678901",
                Gender = "man",
                BirthDate = DateOnly.Parse("08/19/2000"),
            });

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        var spec = new Mock<IIndividualSpecification>();
        spec.Setup(x => x.PageSize).Returns(5);
        spec.Setup(x => x.PageNumber).Returns(2);
        spec.Setup(x => x.Criteria).Returns(x => true);

        var sut = new IndividualsRepository(context);
        // When
        var result = await sut.GetIndividuals(spec.Object);

        // Then
        Assert.Equal(5, result.Count());
        Assert.Equal("name6", result.First().Name);
    }

    [Fact]
    public async Task Test_GetAllIndividuals_With_Name_Criteria()
    {
        // Given
        var entities = new[]
        {
            new IndividualEntity
            {
                Id = 1,
                Name = "john",
                Surname = "doe",
                PersonalId = "12345678901",
                Gender = "man",
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
            new IndividualEntity
            {
                Id = 2,
                Name = "bruce",
                Surname = "wayne",
                PersonalId = "12345678901",
                Gender = "man",
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        var spec = new Mock<IIndividualSpecification>();
        spec.Setup(x => x.Criteria).Returns(x => x.Name == "john");

        var sut = new IndividualsRepository(context);
        // When
        var result = await sut.GetIndividuals(spec.Object);

        // Then
        var list = result.ToList();
        Assert.Single(list);
        Assert.Equal("john", list[0].Name);
        Assert.Equal("doe", list[0].Surname);
    }

    [Fact]
    public async Task Test_GetAllIndividuals_With_Complex_Criteria()
    {
        // Given
        var entities = new[]
        {
            new IndividualEntity
            {
                Id = 1,
                Name = "john",
                Surname = "doe",
                PersonalId = "12345678901",
                Gender = "man",
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
            new IndividualEntity
            {
                Id = 2,
                Name = "bruce",
                Surname = "wayne",
                PersonalId = "92345678901",
                Gender = "man",
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
        };
        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        var spec = new Mock<IIndividualSpecification>();
        spec.Setup(x => x.Criteria)
            .Returns(x => x.Name == "john" && x.Surname == "doe" && x.PersonalId == "12345678901");

        var sut = new IndividualsRepository(context);
        // When
        var result = await sut.GetIndividuals(spec.Object);

        // Then
        var list = result.ToList();
        Assert.Single(list);
        Assert.Equal("john", list[0].Name);
        Assert.Equal("doe", list[0].Surname);
        Assert.Equal("12345678901", list[0].PersonalId);
    }

    [Fact]
    public async Task Test_GetAllIndividuals_With_Empty_Result()
    {
        // Given
        var individuals = new[]
        {
            new IndividualEntity
            {
                Id = 1,
                Name = "john",
                Surname = "doe",
                PersonalId = "12345678901",
                Gender = "man",
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(individuals);
        await context.SaveChangesAsync();

        var spec = new Mock<IIndividualSpecification>();
        spec.Setup(x => x.Criteria).Returns(x => x.Name == "nonexistent");

        var sut = new IndividualsRepository(context);
        // When
        var result = await sut.GetIndividuals(spec.Object);

        // Then
        Assert.Empty(result);
    }
}
