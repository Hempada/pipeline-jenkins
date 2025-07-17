using Business.Services;
using System.Security.Claims;

namespace WebApi.Authorization;

public interface IAuthorizePermissionService
{
    ValueTask<bool> HasAccountPermissionToAsync(params string[] permissions);
}

internal sealed class AuthorizePermissionService : IAuthorizePermissionService
{
    private IAuthService AuthService { get; }

    public IHttpContextAccessor ContextAccessor { get; }

    public AuthorizePermissionService(IAuthService authService, IHttpContextAccessor context)
    {
        AuthService = authService;
        ContextAccessor = context;
    }

    public async ValueTask<bool> HasAccountPermissionToAsync(params string[] permissions)
    {
        if (ContextAccessor.HttpContext is null)
        {
            return false;
        }

        if (!TryGetAccount(out Guid accountId))
        {
            return false;
        }

        try
        {
            var account = await AuthService.GetAccountAsync(accountId);

            if (account.Data?.Profile is null)
            {
                return false;
            }

            return account.Data.Profile.Permissions.Any(x => Array.Exists(permissions, y => y == x));
        }
        catch (Exception)
        {
            return false;
        }

        bool TryGetAccount(out Guid account)
        {
            return Guid.TryParse(ContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Sid)?.Value, out account);
        }
    }
}