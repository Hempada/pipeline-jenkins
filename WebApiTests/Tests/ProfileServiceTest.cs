using Business;
using Business.Services.Impl;
using Microsoft.EntityFrameworkCore;
using WebApiTests.Extensions;
using Xunit;

namespace WebApiTests.Tests
{
    public class ProfileServiceTests
    {
        private readonly ProfileService _profileService;
        private readonly ApplicationDbContext _dbContext;

        public ProfileServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _profileService = new ProfileService(_dbContext);
        }

        [Fact]
        public async Task SaveAsync_WithNonExistentProfileId_ShouldReturnProfileNotFoundError()
        {
            // Arrange
            Guid? id = Guid.NewGuid(); // Um ID que não existe
            string name = "Test Profile";
            var permissions = new List<string> { "Permission1", "Permission2" };
            var token = new CancellationToken();

            // Act
            var result = await _profileService.SaveAsync(id, name, permissions, token);

            // Assert
            Assert.False(result.IsSuccess()); // Verifica que o resultado não é bem-sucedido

            // Verifica se o erro é o esperado
            Assert.NotNull(result.GetErrors());
            Assert.Equal("PROFILE_NOT_FOUND", result.GetErrors().First().Code); // Altere para o valor correto
        }
    }
}
