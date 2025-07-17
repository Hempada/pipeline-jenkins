
using Commons.Data.Results;
using Commons.Models;
using System.Net.Http.Json;

namespace WebUi.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private IHttpClientFactory HttpClientFactory { get; }
        private ISessionManager SessionManager { get; }

        public CustomerService(IHttpClientFactory httpClientFactory, ISessionManager sessionManager)
        {
            HttpClientFactory = httpClientFactory;
            SessionManager = sessionManager;
        }

        public async Task<Result> SaveAsync(Guid? id, string name, string email, CancellationToken token = default)
        {
            using HttpClient httpClient = HttpClientFactory
               .CreateWebApiClient()
               .WithBearerJwt(SessionManager.Token);

            using HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/customer",
                new SaveCustomer(id, name, email), token);

            return await response.ReadResult();
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken token = default)
        {
            using HttpClient httpClient = HttpClientFactory
              .CreateWebApiClient()
              .WithBearerJwt(SessionManager.Token);

            using HttpResponseMessage response = await httpClient.DeleteAsync($"api/customer/{id}", token);

            return await response.ReadResult();
        }
    }
}
