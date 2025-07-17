using Commons.Data.Results;
using Commons.Models;
using OData.QueryBuilder.Conventions.AddressingEntities.Query;
using WebUi.Extensions;

namespace WebUi.Services
{
    public interface IODataService
    {
        ValueTask<Result<ODataCountValue<Account>>> QueryAccountAsync(Action<IODataQueryCollection<Account>> queryFn, string? queryStr = null);
        ValueTask<Result<ODataCountValue<Profile>>> QueryProfileAsync(Action<IODataQueryCollection<Profile>> queryFn, string? queryStr = null);
        ValueTask<Result<ODataCountValue<Customer>>> QueryCustomerAsync(Action<IODataQueryCollection<Customer>> queryFn, string? queryStr = null);
    }
}
