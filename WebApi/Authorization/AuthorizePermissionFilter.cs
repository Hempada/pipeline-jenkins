using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace WebApi.Authorization;

internal sealed class AuthorizePermissionFilter : IAsyncAuthorizationFilter
{
    private IAuthorizePermissionService AuthPermissionService { get; }

    public AuthorizePermissionFilter(IAuthorizePermissionService authorizePermissionService)
    {
        AuthPermissionService = authorizePermissionService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Filters.Any(item => item is IAllowAnonymousFilter))
        {
            return;
        }

        bool isAuthenticated = context.HttpContext.User.Identity?.IsAuthenticated ?? false;

        if (context.Filters.Any(item => item is IAllowApiKeyAuthenticationFilter) && isAuthenticated)
        {
            return;
        }

        IEnumerable<RequiresPermissionAttribute> permissions = context.ActionDescriptor.EndpointMetadata
            .Where(x => x is RequiresPermissionAttribute)
            .Select(x => (x as RequiresPermissionAttribute)!);

        if (!permissions.Any())
        {
            return;
        }

        if (!Guid.TryParse(context.HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value, out _))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        bool hasPermission = await AuthPermissionService.HasAccountPermissionToAsync(permissions.SelectMany(x => x.Permissions).ToArray());

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}