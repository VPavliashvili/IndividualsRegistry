using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Enums;
using IndividualsRegistry.Infrastructure.Data;
using IndividualsRegistry.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Xunit.Abstractions;

namespace IndividualsRegistry.Infrastructure.Test.Repositories;

public class UnitOfWorkTests
{
    private readonly ITestOutputHelper _output;

    public UnitOfWorkTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task Test_SaveChanges_Returns_Number_Of_Affected_Rows()
    {
        // Given
        var context = new Mock<IndividualsDbContext>();
        context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(5);

        var repo = new Mock<IIndividualsRepository>();

        var sut = new UnitOfWork(repo.Object, context.Object);
        // When
        var result = await sut.SaveChanges();

        // Then
        Assert.Equal(5, result);
        context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
