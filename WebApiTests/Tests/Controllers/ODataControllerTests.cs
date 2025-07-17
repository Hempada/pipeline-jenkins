using Business;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Controllers;
using Xunit;

namespace WebApiTests.Tests.Controllers
{
    public class ODataControllerTests
    {
        private readonly ODataController _controller;
        private readonly ApplicationDbContext _context;

        public ODataControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>() // Alterado para ApplicationDbContext
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new ODataController();
        }

        [Fact]
        public void GetAccounts_ReturnsOkResult_WithAccounts()
        {
            // Arrange
            var accounts = new List<Account>
    {
        new Account
        {
            Id = Guid.NewGuid(),
            Name = "Account1",     // Definindo o Name
            Username = "User1",    // Definindo o Username
            Email = "user1@example.com" // Definindo o Email
        },
        new Account
        {
            Id = Guid.NewGuid(),
            Name = "Account2",     // Definindo o Name
            Username = "User2",    // Definindo o Username
            Email = "user2@example.com" // Definindo o Email
        }
    };

            _context.Accounts.AddRange(accounts);
            _context.SaveChanges();

            // Act
            var result = _controller.GetAccounts(_context);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAccounts = Assert.IsAssignableFrom<IEnumerable<Account>>(okResult.Value);
            Assert.Equal(2, returnedAccounts.Count());
        }



        [Fact]
        public void GetProfiles_ReturnsOkResult_WithProfiles()
        {
            // Arrange
            var profiles = new List<Business.Models.Profile>
    {
            new Business.Models.Profile { Id = Guid.NewGuid(), Name = "Profile1", Active = true, Permissions = new List<string>() },
            new Business.Models.Profile { Id = Guid.NewGuid(), Name = "Profile2", Active = true, Permissions = new List<string>() }
    };

            _context.Profiles.AddRange(profiles);
            _context.SaveChanges();

            // Act
            var result = _controller.GetProfiles(_context);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProfiles = Assert.IsAssignableFrom<IEnumerable<Business.Models.Profile>>(okResult.Value);
            Assert.Equal(2, returnedProfiles.Count());
        }

        [Fact]
        public void GetCustomers_ReturnsOkResult_WithCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), Name = "Customer1", Email = "customer1@example.com" },
                new Customer { Id = Guid.NewGuid(), Name = "Customer2", Email = "customer2@example.com" }
            };

            _context.Customers.AddRange(customers);
            _context.SaveChanges();

            // Act
            var result = _controller.GetCustomers(_context);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);
            Assert.Equal(2, returnedCustomers.Count());
        }
    }
}
