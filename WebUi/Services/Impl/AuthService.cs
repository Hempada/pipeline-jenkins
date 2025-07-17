using Commons.Data.Results;
using Commons.Models;
using System.Net.Http.Json;

namespace WebUi.Services.Impl
{
    public class AuthService : IAuthService
    {
        private IHttpClientFactory HttpClientFactory { get; }

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public async Task<Result<Session?>> AuthAsync(string username, string password)
        {
            using HttpClient httpClient = HttpClientFactory
               .CreateWebApiClient();

            var model = new AuthenticateUser(username, password);

            using HttpResponseMessage response = await httpClient.PostAsJsonAsync($"api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Session>();
                return Result.Ok(result);
            }

            return Result.Fail("AuthAsync", response.ReasonPhrase!);
        }

        public async Task<Result<Session?>> GetAccountAsync(string token)
        {
            using HttpClient httpClient = HttpClientFactory
               .CreateWebApiClient()
               .WithBearerJwt(token);

            using HttpResponseMessage response = await httpClient.GetAsync($"api/auth/get-account");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Session>();
                return Result.Ok(result);
            }

            return Result.Fail("AuthAsync", response.ReasonPhrase!);
        }

        public Task<Result> RegisterAsync(string name, string username, string email, string password)
        {
            return Task.FromResult(Result.Ok());
        }
    }
}
