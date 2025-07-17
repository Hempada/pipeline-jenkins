
using Commons.Data.Results;
using Commons.Models;
using System.Net.Http.Json;

namespace WebUi.Services.Impl
{
    public class ProfileService : IProfileService
    {
        private IHttpClientFactory HttpClientFactory { get; }
        private ISessionManager SessionManager { get; }

        public ProfileService(IHttpClientFactory httpClientFactory, ISessionManager sessionManager)
        {
            HttpClientFactory = httpClientFactory;
            SessionManager = sessionManager;
        }

        public async Task<Result> SaveAsync(Guid? id, string name, IEnumerable<string> permissions, CancellationToken token = default)
        {
            using HttpClient httpClient = HttpClientFactory
               .CreateWebApiClient()
               .WithBearerJwt(SessionManager.Token);

            using HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/profile",
                new SaveProfile(id, name, permissions), token);

            return await response.ReadResult();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken token = default)
        {
            using HttpClient httpClient = HttpClientFactory
              .CreateWebApiClient()
              .WithBearerJwt(SessionManager.Token);

            using HttpResponseMessage response = await httpClient.DeleteAsync($"api/profile/{id}", token);

            return await response.ReadResult();
        }
    }
}
