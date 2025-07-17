using Business;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Extensions;

namespace WebApiTests.Tests.Controllers
{
    public abstract class ControllerTests
    {
        protected readonly ServiceProvider ServiceProvider;
        protected readonly WebApplicationBuilder Builder;
        protected readonly ApplicationDbContext Database;

        public ControllerTests()
        {
            Builder = WebApplication.CreateBuilder([]);
            var connectionString = Builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            var token = Builder.Configuration.GetSection("AppSettings:Token").Value ?? throw new InvalidOperationException("Connection string 'AppSettings:Token' not found.");

            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure(connectionString, token);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            Database = ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }
}


