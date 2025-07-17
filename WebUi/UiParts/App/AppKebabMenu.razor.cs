using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebUi.Services;

namespace WebUi.UiParts.App;

public partial class AppKebabMenu : ComponentBase
{
    [Inject]
    ISessionManager SessionManager { get; set; } = default!;

    [Parameter]
    public IEnumerable<AppKebabItem> Items { get; set; } = Enumerable.Empty<AppKebabItem>();

    [Parameter]
    public bool Dense { get; set; } = true;

    [Parameter]
    public Color Color { get; set; } = Color.Default;

    [Parameter]
    public Origin AnchorOrigin { get; set; } = Origin.TopRight;

    [Parameter]
    public Origin TransformOrigin { get; set; } = Origin.TopRight;

    [Parameter]
    public bool Disabled { get; set; } = false;

    private bool Forbidden { get; set; } = false;

    private MudMenu MenuRef { get; set; } = default!;

    private void OnClick(AppKebabItem item)
    {
        item.OnClick();
        MenuRef.CloseMenuAsync();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        string[] permissions = Items
            .Select(x => x.Permissions)
            .SelectMany(s => s)
            .Distinct()
            .ToArray();

        Forbidden = !(permissions.Length == 0 || SessionManager.HasAccountPermissionTo(permissions));
    }
}

public record AppKebabItem(string Text, Action OnClick, string[] Permissions, string? Icon = null);