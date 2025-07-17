using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization;

public sealed class RequiresPermissionAttribute : AuthorizeAttribute
{
    public RequiresPermissionAttribute(params string[] permissions)
    {
        Permissions = permissions;
    }

    public string[] Permissions { get; }
}