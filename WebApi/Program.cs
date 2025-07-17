using WebApi.Extensions;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            var jwtToken = builder.Configuration.GetSection("AppSettings:Token").Value ?? throw new InvalidOperationException("Connection string 'AppSettings:Token' not found.");

            builder.Services.Configure(connectionString, jwtToken);

            var app = builder.Build().WithDefaultConfigurations();
            await app.RunAsync();
        }
    }
}
