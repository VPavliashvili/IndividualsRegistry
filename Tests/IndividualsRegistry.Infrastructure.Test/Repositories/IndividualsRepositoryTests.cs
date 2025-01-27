using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Enums;
using IndividualsRegistry.Domain.Exceptions;
using IndividualsRegistry.Domain.Specifications;
using IndividualsRegistry.Infrastructure.Data;
using IndividualsRegistry.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit.Abstractions;

namespace IndividualsRegistry.Infrastructure.Test.Repositories;

public class IndividualsRepositoryTests
{
    private readonly ITestOutputHelper _output;

    private static DbContextOptions<IndividualsDbContext> DbContextOptions =>
        new DbContextOptionsBuilder<IndividualsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

    public IndividualsRepositoryTests(ITestOutputHelper output)
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
            Gender = Gender.Male,
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
            Gender = Gender.Male,
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
            Gender = Gender.Male,
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
            Gender = Gender.Male,
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
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        var sut = new IndividualsRepository(context);
        // When
        await sut.UpdateIndividual(updated);
        await context.SaveChangesAsync();

        // Then
        var resutl = await context.Individuals.FindAsync(1);

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
            Gender = Gender.Male,
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
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var entity2 = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "10987654321",
            Gender = Gender.Male,
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
            Gender = Gender.Male,
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
            Gender = Gender.Male,
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
            Gender = Gender.Male,
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
            Gender = Gender.Male,
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
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var entity2 = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "12345678901",
            Gender = Gender.Male,
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
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var related = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "12345678901",
            Gender = Gender.Male,
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
                Gender = Gender.Male,
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
            new IndividualEntity
            {
                Id = 2,
                Name = "bruce",
                Surname = "wayne",
                PersonalId = "12345678901",
                Gender = Gender.Male,
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
    public async Task Test_GetIndividuals_WithPagination_ShouldReturnCorrectPage()
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
                Gender = Gender.Male,
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
    public async Task Test_GetIndividuals_With_Name_Criteria()
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
                Gender = Gender.Male,
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
            new IndividualEntity
            {
                Id = 2,
                Name = "bruce",
                Surname = "wayne",
                PersonalId = "12345678901",
                Gender = Gender.Male,
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        var spec = new Mock<IIndividualSpecification>();
        spec.Setup(x => x.PageNumber).Returns(1);
        spec.Setup(x => x.PageSize).Returns(100);
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
    public async Task Test_GetIndividuals_With_Complex_Criteria()
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
                Gender = Gender.Male,
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
            new IndividualEntity
            {
                Id = 2,
                Name = "bruce",
                Surname = "wayne",
                PersonalId = "92345678901",
                Gender = Gender.Male,
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
        };
        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        var spec = new Mock<IIndividualSpecification>();
        spec.Setup(x => x.PageNumber).Returns(1);
        spec.Setup(x => x.PageSize).Returns(100);
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
    public async Task Test_GetIndividuals_With_Empty_Result()
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
                Gender = Gender.Male,
                BirthDate = DateOnly.Parse("08/19/2000"),
            },
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(individuals);
        await context.SaveChangesAsync();

        var spec = new Mock<IIndividualSpecification>();
        spec.Setup(x => x.PageSize).Returns(123);
        spec.Setup(x => x.PageNumber).Returns(1);
        spec.Setup(x => x.Criteria).Returns(x => x.Name == "nonexistent");

        var sut = new IndividualsRepository(context);
        // When
        var result = await sut.GetIndividuals(spec.Object);

        // Then
        Assert.Empty(result);
    }

    [Fact]
    public async Task Test_GetIndividual_Returns_Individual_When_Exists()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddAsync(entity);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        var result = await sut.GetIndividual(1);

        // Then
        Assert.NotNull(result);
        Assert.Equal("john", result.Name);
        Assert.Equal("doe", result.Surname);
        Assert.Equal("12345678901", result.PersonalId);
    }

    [Fact]
    public async Task Test_GetIndividual_Returns_Null_When_Not_Exists()
    {
        // Given
        var nonExistentId = 123;

        var context = new Mock<IndividualsDbContext>(DbContextOptions);
        context.Setup(x => x.Individuals).ReturnsDbSet([]);

        var sut = new IndividualsRepository(context.Object);
        // When
        var result = await sut.GetIndividual(nonExistentId);

        // Then
        Assert.Null(result);
    }

    [Fact]
    public async Task Test_GetIndividual_Includes_Related_Individuals_Data()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
            RelatedIndividuals =
            [
                new()
                {
                    Id = 2,
                    Name = "bruce",
                    Surname = "wayne",
                    PersonalId = "92345678901",
                    Gender = Gender.Male,
                    BirthDate = DateOnly.Parse("08/19/2000"),
                },
            ],
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddAsync(entity);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        var result = await sut.GetIndividual(1);

        // Then
        Assert.NotNull(result);
        Assert.NotNull(result.RelatedIndividuals);
    }

    [Fact]
    public async Task Test_AddRelatedIndividual_Successfully_Adds_Relation()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var relatedEntity = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "92345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entity, relatedEntity);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        await sut.AddRelatedIndividual(1, relatedEntity.Id, RelationType.Colleague);
        await context.SaveChangesAsync();

        // Then
        var updatedIndividual = await context
            .Individuals.Include(x => x.RelatedIndividuals)
            .FirstAsync(x => x.Id == 1);

        Assert.Single(updatedIndividual.RelatedIndividuals!);
        Assert.Contains(updatedIndividual.RelatedIndividuals!, x => x.Id == 2);
    }

    [Fact]
    public async Task Test_AddRelatedIndividual_Throws_When_Individual_Not_Found()
    {
        // Given
        var relatedIndividual = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddAsync(relatedIndividual);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        var ex = await Record.ExceptionAsync(
            async () =>
                await sut.AddRelatedIndividual(123, relatedIndividual.Id, RelationType.Colleague)
        );

        // Then
        Assert.IsType<DoesNotExistException>(ex);
    }

    [Fact]
    public async Task Test_AddRelatedIndividual_Throws_When_Related_Individual_Not_Found()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var nonExistentRelated = new IndividualEntity
        {
            Id = 123,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "92345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddAsync(entity);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        var ex = await Record.ExceptionAsync(
            async () =>
                await sut.AddRelatedIndividual(1, nonExistentRelated.Id, RelationType.Colleague)
        );

        // Then
        Assert.IsType<DoesNotExistException>(ex);
    }

    [Fact]
    public async Task Test_AddRelatedIndividual_Prevents_Self_Relation()
    {
        // Given
        var individual = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddAsync(individual);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        var ex = await Record.ExceptionAsync(
            async () => await sut.AddRelatedIndividual(1, individual.Id, RelationType.Colleague)
        );

        // Then
        Assert.IsType<InvalidOperationException>(ex);
    }

    [Fact]
    public async Task Test_AddRelatedIndividual_Throws_When_Relation_Already_Exists()
    {
        // Given
        var individual = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var relatedIndividual = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "92345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(individual, relatedIndividual);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        await sut.AddRelatedIndividual(1, relatedIndividual.Id, RelationType.Relative);
        await context.SaveChangesAsync();
        // When
        var ex = await Record.ExceptionAsync(
            async () =>
                await sut.AddRelatedIndividual(1, relatedIndividual.Id, RelationType.Colleague)
        );

        // Then
        Assert.IsType<RelatedIndividualAlreadyExists>(ex);
    }

    [Fact]
    public async Task Test_RemoveRelatedIndividual_Successfully_Removes_Relation()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var relatedIndividual = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "92345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entity, relatedIndividual);
        await context.SaveChangesAsync();

        var relation = new RelationEntity
        {
            IndividualId = 1,
            RelatedIndividualId = 2,
            RelationType = RelationType.Colleague,
        };
        await context.Relations.AddAsync(relation);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        await sut.RemoveRelatedIndividual(entity.Id, relatedIndividual.Id);
        await context.SaveChangesAsync();

        // Then
        var relations = await context.Relations.ToListAsync();
        Assert.Empty(relations);
    }

    [Fact]
    public async Task Test_RemoveRelatedIndividual_Throws_When_Individual_Not_Found()
    {
        // Given
        var relatedIndividual = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var wrongId = 123;

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddAsync(relatedIndividual);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        var ex = await Record.ExceptionAsync(
            async () => await sut.RemoveRelatedIndividual(wrongId, relatedIndividual.Id)
        );

        // Then
        Assert.IsType<DoesNotExistException>(ex);
    }

    [Fact]
    public async Task Test_RemoveRelatedIndividual_Throws_When_Related_Individual_Not_Found()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        int wrongId = 123;

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddAsync(entity);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        var ex = await Record.ExceptionAsync(
            async () => await sut.RemoveRelatedIndividual(entity.Id, wrongId)
        );

        // Then
        Assert.IsType<DoesNotExistException>(ex);
    }

    [Fact]
    public async Task Test_RemoveRelatedIndividual_Throws_When_Relation_Not_Exists()
    {
        // Given
        var entity = new IndividualEntity
        {
            Id = 1,
            Name = "john",
            Surname = "doe",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };
        var relatedIndividual = new IndividualEntity
        {
            Id = 2,
            Name = "bruce",
            Surname = "wayne",
            PersonalId = "12345678901",
            Gender = Gender.Male,
            BirthDate = DateOnly.Parse("08/19/2000"),
        };

        using var context = new IndividualsDbContext(DbContextOptions);
        await context.Individuals.AddRangeAsync(entity, relatedIndividual);
        await context.SaveChangesAsync();

        var sut = new IndividualsRepository(context);
        // When
        var ex = await Record.ExceptionAsync(
            async () => await sut.RemoveRelatedIndividual(entity.Id, relatedIndividual.Id)
        );
        // Then
        Assert.IsType<RelationDoesNotExistException>(ex);
    }
}
