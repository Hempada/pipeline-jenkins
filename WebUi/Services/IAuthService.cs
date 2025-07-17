using Commons.Data.Results;
using Commons.Models;

namespace WebUi.Services
{
    public interface IAuthService
    {
        Task<Result<Session?>> AuthAsync(string username, string password);
        Task<Result<Session?>> GetAccountAsync(string token);
        Task<Result> RegisterAsync(string name, string username, string email, string password);
    }
}
