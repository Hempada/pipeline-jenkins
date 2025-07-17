using WebUi.Services.Impl;
using WebUi.Services;
using WebUi.UiParts;
using MudBlazor.Services;
using Blazored.LocalStorage;

namespace WebUi.Extensions
{
    public static class ProgramExtensions
    {
        public static IServiceCollection Configure(this IServiceCollection services, string host)
        {
            services.AddBlazorBootstrap();
            services.AddMudServices();
            services.AddBlazoredLocalStorage();

            services.AddWebApiHttpClient(new Uri(host));

            services.AddScoped<IInternalSessionManager, SessionManager>();
            services.AddScoped<ISessionManager>(sp => sp.GetRequiredService<IInternalSessionManager>());
            services.AddScoped<IAppSnackbar, AppSnackbar>();
            services.AddScoped<IODataService, ODataService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProfileService, ProfileService>();

            return services;
        }
    }
}
