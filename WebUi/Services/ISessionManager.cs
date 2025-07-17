using Commons.Models;

namespace WebUi.Services
{
    public interface ISessionManager
    {
        Account Account { get; }
        string Token { get; }
        bool HasAccountPermissionTo(params string[] permissions);
    }

    public interface IInternalSessionManager : ISessionManager
    {
        bool IsValid();
        Task InitializeAsync();
        Task SetSessionAsync(Session Session);
        Task ClearSessionAsync();
    }
}
