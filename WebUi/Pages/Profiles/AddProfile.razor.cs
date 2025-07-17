using Commons.Data;
using Commons.Data.Results;
using Commons.Models;
using Microsoft.AspNetCore.Components;
using WebUi.Extensions;
using WebUi.Services;
using WebUi.UiParts.Base;

namespace WebUi.Pages.Profiles;

public partial class AddProfile : AddPageComponentBase<Profile>
{
    #region INJECTS
    [Inject] protected IProfileService ProfileService { get; init; } = default!;
    #endregion INJECTS

    #region PROPS
    private OnChangeValue<string> Name { get; set; } = default!;
    private OnChangeValue<IEnumerable<string>> Permissions { get; set; } = default!;
    #endregion PROPS

    #region OVERRIDES
    protected override string TypeName() => "Perfil";

    protected override async ValueTask<Result<ODataCountValue<Profile>>> QueryItemAsync(
       IODataService service, Guid id)
    {
        return await service.QueryProfileAsync(
            query =>
            {
                query.Filter(x => x.Id == id);
            }
        );
    }

    protected override async Task<Result> SaveItemAsync()
    {
        return await ProfileService.SaveAsync(ItemId, Name.Value, Permissions.Value);
    }
    #endregion OVERRIDES

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!Edit)
        {
            Name = new OnChangeValue<string>(OnDataChanged);
            Permissions = new OnChangeValue<IEnumerable<string>>(OnDataChanged);
        }
    }

    protected override void OnLoadedItem(Profile profile)
    {
        Name = new OnChangeValue<string>(profile.Name, OnDataChanged);
        Permissions = new OnChangeValue<IEnumerable<string>>(profile.Permissions, OnDataChanged);
    }

    private static string GetMultiSelectionText(List<string> selectedValues)
    {
        return string.Join(", ", selectedValues.Select(x => Permission.GetDescription(x)));
    }
}
