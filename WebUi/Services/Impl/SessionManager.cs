using Blazored.LocalStorage;
using Commons.Models;

namespace WebUi.Services.Impl
{
    public class SessionManager : IInternalSessionManager
    {
        private ILocalStorageService LocalStorage { get; }
        private IAuthService AuthService { get; }

        public SessionManager(ILocalStorageService localStorage, IAuthService authService)
        {
            LocalStorage = localStorage;
            AuthService = authService;
        }

        public async Task InitializeAsync()
        {
            token = await LocalStorage.GetItemAsStringAsync("Token");
            if (!string.IsNullOrEmpty(token))
            {
                var result = await AuthService.GetAccountAsync(token);
                if (result is not null && result.Data is not null)
                {
                    await SetSessionAsync(result.Data);
                    return;
                }
            }

            await ClearSessionAsync();
        }

        public async Task ClearSessionAsync()
        {
            await LocalStorage.RemoveItemAsync("Token");
            account = null;
            token = null;
        }

        public async Task SetSessionAsync(Session Session)
        {
            await LocalStorage.SetItemAsStringAsync("Token", Session.Token);
            account = Session.Account;
            token = Session.Token;
        }
        public bool IsValid()
        {
            return account is not null && token is not null;
        }


        private Account? account = null;
        public Account Account
        {
            get
            {
                if (account is null)
                {
                    throw new InvalidOperationException("Account should be accessible only after authentication.");
                }
                return account;
            }
        }

        private string? token = null;
        public string Token
        {
            get
            {
                if (token is null)
                {
                    throw new InvalidOperationException("Account should be accessible only after authentication.");
                }
                return token;
            }
        }

        public bool HasAccountPermissionTo(params string[] permissions)
        {
            if (permissions.Length == 0)
            {
                return true;
            }

            if (account is null)
            {
                return false;
            }

            return account.Profile?.Permissions.Any(p => permissions.Contains(p)) ?? false;
        }
    }
}
