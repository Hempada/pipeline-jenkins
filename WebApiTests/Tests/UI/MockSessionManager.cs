using Commons.Data.Results;
using Commons.Models;
using WebUi.Services;

public class MockSessionManager : IInternalSessionManager
{
    private readonly Session _session;

    public MockSessionManager()
    {
        var account = new Account(
            Guid.NewGuid(),           // Id
            "Test User",              // Name
            "test.user@example.com",  // Email
            "1234567890",             // Phone
            null                      // Profile (ou use um Profile mockado se necessário)
        );

        _session = new Session(account, "fake-token");
    }

    public string Token => _session.Token;

    public Account Account => _session.Account;

    public bool IsValid()
    {
        return true;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task<Result<Session>> GetSessionAsync()
    {
        return Task.FromResult(Result.Ok(_session));
    }

    public Task SetSessionAsync(Session session)
    {
        return Task.CompletedTask;
    }

    public Task ClearSessionAsync()
    {
        return Task.CompletedTask;
    }

    public bool HasAccountPermissionTo(params string[] permissions)
    {
        return true; // Retorne true ou false dependendo do comportamento esperado no teste
    }
}
