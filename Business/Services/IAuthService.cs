using Business.Models;
using Commons.Data.Results;

namespace Business.Services
{
    public interface IAuthService
    {
        Task<Result<Account>> AuthAsync(string username, string password, CancellationToken token = default);
        Task<Result<Account>> GetAccountAsync(Guid id, CancellationToken token = default);
        Task<Result> RegisterAsync(string name, string username, string email, string password, CancellationToken token = default);
    }
}
