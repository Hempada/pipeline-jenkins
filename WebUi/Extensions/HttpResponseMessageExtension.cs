using Commons.Data.Results;
using System.Net.Http.Json;

namespace Microsoft.Extensions.DependencyInjection;

public static class HttpResponseMessageExtension
{
    public static async Task<Result> ReadResult(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return Result.Ok();
        }

        var errors = await response.Content.ReadFromJsonAsync<IEnumerable<Error>>();
        if (errors is null || !errors.Any())
        {
            return Result.Fail("Erro desconhecido", response.ReasonPhrase!);
        }

        return Result.Fail(errors);
    }
}