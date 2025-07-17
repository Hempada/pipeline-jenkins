using Microsoft.AspNetCore.Components;
using WebUi.Services;
using WebUi.UiParts.Base;

namespace WebUi.Pages.Auth
{
    public partial class Login : PageComponentBase
    {
        [Inject] private IAuthService AuthService { get; init; } = default!;
        [Inject] private IInternalSessionManager SessionManager { get; init; } = default!;
        [Inject] private NavigationManager NavigationManager { get; init; } = default!;

        private string Username { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;

        private async Task QueryLoginAsync()
        {
            Loading = true;

            var result = await AuthService.AuthAsync(Username, Password);

            if (!result.Valid)
            {
                Loading = false;
                return;
            }

            if (result.Data is null || result.Data.Token is null)
            {

                Loading = false;
                return;
            }

            await SessionManager.SetSessionAsync(result.Data);
            NavigationManager.NavigateTo("");
        }
    }
}
