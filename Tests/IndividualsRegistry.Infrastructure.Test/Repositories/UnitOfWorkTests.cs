using IndividualsRegistry.Application.Contracts;
using IndividualsRegistry.Infrastructure.Data;
using IndividualsRegistry.Infrastructure.Repositories;
using Moq;

namespace IndividualsRegistry.Infrastructure.Test.Repositories;

public class UnitOfWorkTests
{
    public UnitOfWorkTests() { }

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
