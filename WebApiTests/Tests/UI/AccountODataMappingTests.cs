using Business.Mappings;
using Business.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WebApiTests.Tests
{
    public class AccountODataMappingTests
    {
        [Fact]
        public void Configure_ShouldConfigureEntityCorrectly()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MockDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new MockDbContext(options);
            var modelBuilder = new ModelBuilder(); // Cria um ModelBuilder
            var accountMapping = new AccountMapping();
            accountMapping.Configure(modelBuilder.Entity<Account>()); // Passa a entidade Account

            // Act
            var entityType = modelBuilder.Model.GetEntityTypes().First(e => e.ClrType == typeof(Account));

            // Assert
            Assert.NotNull(entityType.FindPrimaryKey());
            Assert.Equal(6, entityType.GetProperties().Count());
        }
    }

    // Mock DbContext
    public class MockDbContext : DbContext
    {
        public MockDbContext(DbContextOptions<MockDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; } // Adicione o DbSet para Account
    }
}
