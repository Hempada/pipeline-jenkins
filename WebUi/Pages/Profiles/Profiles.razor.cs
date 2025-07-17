using BlazorBootstrap;
using Commons.Data.Results;
using Commons.Models;
using Microsoft.AspNetCore.Components;
using WebUi.Extensions;
using WebUi.Services;
using WebUi.UiParts.App;
using WebUi.UiParts.Base;

namespace WebUi.Pages.Profiles;

public partial class Profiles : ListPageComponentBase<Profile>
{
    #region INJECTS
    [Inject] protected IProfileService ProfileService { get; init; } = default!;
    #endregion INJECTS

    #region PROPS
    private AppGrid<Profile> grid { get; set; } = default!;
    #endregion PROPS

    #region OVERRIDES
    protected override AppGrid<Profile> Grid() => grid;
    protected override string[] AddButtonPermissions() => [Permission.CREATE_PROFILE];
    protected override string TypeName() => "Perfil";
    protected override string TypeNamePlural() => "Perfis";

    protected override async ValueTask<Result<ODataCountValue<Profile>>> QueryItemsAsync(
       IODataService service, IEnumerable<FilterItem> filters, string queryStr)
    {
        return await service.QueryProfileAsync(
            query => { },
            queryStr
        );
    }

    protected override async Task<Result> DeleteItemAsync(Guid id)
    {
        return await ProfileService.DeleteAsync(id);
    }
    #endregion OVERRIDES
}
