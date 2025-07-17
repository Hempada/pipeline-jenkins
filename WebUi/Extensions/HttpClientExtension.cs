using System.Net.Http.Headers;

namespace Microsoft.Extensions.DependencyInjection;

public static class HttpClientExtension
{
    private const string WEB_API = "WEB_API";

    public static IHttpClientBuilder AddWebApiHttpClient(this IServiceCollection services, Uri baseAddress)
    {
        return services.AddHttpClient(WEB_API, client =>
        {
            client.BaseAddress = baseAddress;
        });
    }

    public static HttpClient CreateWebApiClient(this IHttpClientFactory httpClientFactory)
    {
        return httpClientFactory.CreateClient(WEB_API);
    }

    public static HttpClient WithBearerJwt(this HttpClient httpClient, string token)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return httpClient;
    }
}