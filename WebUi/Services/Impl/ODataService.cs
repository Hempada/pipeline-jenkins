using Commons.Data.Results;
using Commons.Models;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.AddressingEntities.Query;
using System.Net.Http.Json;
using WebUi.Extensions;

namespace WebUi.Services.Impl
{
    public class ODataService : IODataService
    {
        private IHttpClientFactory HttpClientFactory { get; }
        private ISessionManager SessionManager { get; }

        public ODataService(IHttpClientFactory httpClientFactory, ISessionManager sessionManager)
        {
            HttpClientFactory = httpClientFactory;
            SessionManager = sessionManager;
        }

        public async ValueTask<Result<ODataCountValue<Account>>> QueryAccountAsync(Action<IODataQueryCollection<Account>> queryFn, string? queryStr = null)
        {
            using HttpClient httpClient = HttpClientFactory
                .CreateWebApiClient()
                .WithBearerJwt(SessionManager.Token);

            IODataQueryCollection<Account> queryBuilder = new ODataQueryBuilder(httpClient.BaseAddress)
                .For<Account>("odata/accounts")
                .ByList()
                .Count();

            queryFn.Invoke(queryBuilder);

            ODataCountValue<Account>? queryResult = await httpClient
                .GetFromJsonAsync<ODataCountValue<Account>>(queryBuilder.ToUri() + queryStr);

            if (queryResult is null)
            {
                return Result.Fail("Accounts", "Could not get data");
            }

            return Result.Ok(queryResult);
        }

        public async ValueTask<Result<ODataCountValue<Profile>>> QueryProfileAsync(Action<IODataQueryCollection<Profile>> queryFn, string? queryStr = null)
        {
            using HttpClient httpClient = HttpClientFactory
                .CreateWebApiClient()
                .WithBearerJwt(SessionManager.Token);

            IODataQueryCollection<Profile> queryBuilder = new ODataQueryBuilder(httpClient.BaseAddress)
                .For<Profile>("odata/profiles")
                .ByList()
                .Count();

            queryFn.Invoke(queryBuilder);

            ODataCountValue<Profile>? queryResult = await httpClient
                .GetFromJsonAsync<ODataCountValue<Profile>>(queryBuilder.ToUri() + queryStr);

            if (queryResult is null)
            {
                return Result.Fail("Profiles", "Could not get data");
            }

            return Result.Ok(queryResult);
        }

        public async ValueTask<Result<ODataCountValue<Customer>>> QueryCustomerAsync(Action<IODataQueryCollection<Customer>> queryFn, string? queryStr = null)
        {
            using HttpClient httpClient = HttpClientFactory
                .CreateWebApiClient()
                .WithBearerJwt(SessionManager.Token);

            IODataQueryCollection<Customer> queryBuilder = new ODataQueryBuilder(httpClient.BaseAddress)
                .For<Customer>("odata/customers")
                .ByList()
                .Count();

            queryFn.Invoke(queryBuilder);

            ODataCountValue<Customer>? queryResult = await httpClient
                .GetFromJsonAsync<ODataCountValue<Customer>>(queryBuilder.ToUri() + queryStr);

            if (queryResult is null)
            {
                return Result.Fail("Customers", "Could not get data");
            }

            return Result.Ok(queryResult);
        }
    }
}
