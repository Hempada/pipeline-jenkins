using Microsoft.AspNetCore.Authorization;

namespace WebUi.Authorization;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class RequiresPermissionAttribute : AuthorizeAttribute
{
    public RequiresPermissionAttribute(params string[] permissions)
    {
        Permissions = permissions;
    }

    public string[] Permissions { get; }
}