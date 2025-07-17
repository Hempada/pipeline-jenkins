using Business;
using Business.Services;
using Business.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using System.Text.Json;
using WebApi.Authorization;

namespace WebApi.Extensions
{
    public static class ProgramExtensions
    {
        #region IServiceCollection
        public static IServiceCollection Configure(this IServiceCollection services, string connectionString, string jwtToken)
        {
            services
                .AddDatabase(connectionString)
                .AddControllersAndOData("odata")
                .AddJwtAuthentication(jwtToken)
                .AddSwagger();

            services.AddCors();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddScoped<IAuthorizePermissionService, AuthorizePermissionService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddControllersAndOData(this IServiceCollection services, string odataRoutePrefix)
        {
            services
                .AddAuthorization()
                .AddHttpContextAccessor()
                .AddControllers(options => options.Filters.Add<AuthorizePermissionFilter>())
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = true;
                })
                .AddOData(options =>
                {
                    options.Conventions.Remove(options.Conventions.OfType<MetadataRoutingConvention>().First());
                    options.AddRouteComponentsFromAssembly(odataRoutePrefix, Assembly.GetEntryAssembly()!);
                    options.EnableQueryFeatures();
                });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string jwtToken)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtToken)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            return services;
        }
        #endregion IServiceCollection

        #region WebApplication
        public static WebApplication WithDefaultConfigurations(this WebApplication app)
        {
            app.UseHttpsRedirection();

            app
                .WithControllers()
                .WithOData()
                .WithBlazor()
                .WithSwagger();

            return app;
        }


        public static WebApplication WithSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            return app;
        }

        public static WebApplication WithControllers(this WebApplication app)
        {
            app.MapControllers();
            return app;
        }

        public static WebApplication WithOData(this WebApplication app)
        {
            app.UseRouting();
            app.UseAuthorization();
            return app;
        }

        public static WebApplication WithBlazor(this WebApplication app)
        {
            if (!app.Environment.IsProduction())
            {
                app.UseWebAssemblyDebugging();
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.MapFallbackToFile("index.html");
            return app;
        }
        #endregion WebApplication
    }
}
