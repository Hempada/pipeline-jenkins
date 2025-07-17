
using Commons.Data.Results;
using Commons.Models;
using System.Net.Http.Json;

namespace WebUi.Services.Impl
{
    public class AccountService : IAccountService
    {
        private IHttpClientFactory HttpClientFactory { get; }
        private ISessionManager SessionManager { get; }

        public AccountService(IHttpClientFactory httpClientFactory, ISessionManager sessionManager)
        {
            HttpClientFactory = httpClientFactory;
            SessionManager = sessionManager;
        }

        public async Task<Result> SaveAsync(Guid? id, string name, string username, string email, Guid profileId, CancellationToken token = default)
        {
            using HttpClient httpClient = HttpClientFactory
               .CreateWebApiClient()
               .WithBearerJwt(SessionManager.Token);

            using HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/account",
                new SaveAccount(id, name, username, email, profileId), token);

            return await response.ReadResult();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken token = default)
        {
            using HttpClient httpClient = HttpClientFactory
              .CreateWebApiClient()
              .WithBearerJwt(SessionManager.Token);

            using HttpResponseMessage response = await httpClient.DeleteAsync($"api/account/{id}", token);

            return await response.ReadResult();
        }
    }
}
